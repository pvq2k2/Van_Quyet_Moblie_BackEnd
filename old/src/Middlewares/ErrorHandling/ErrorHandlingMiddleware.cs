using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Middlewares.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ApiBehaviorOptions _options;
        private readonly IServiceProvider _serviceProvider;
        public ErrorHandlingMiddleware(RequestDelegate next, IOptions<ApiBehaviorOptions> options, IServiceProvider serviceProvider)
        {
            this.next = next;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.Now;
            context.Response.OnStarting(() =>
            {
                var duration = (int)(DateTime.Now - startTime).TotalMilliseconds;
                return CreateLog(context, duration);
            });
            try
            {
                await next(context);
            }
            catch (CustomException ex)
            {
                string result = await CreateProblemDetails(httpContext: context, statusCode: ex.ErrorCode, detail: ex.ErrorMessage);
                await context.Response.WriteAsync(result);
            }
            catch (SqlException ex)
            {
                await SplitError(ex.Message, context);
            }
            catch (Exception ex)
            {
                await SplitError(ex.Message, context);
            }
        }


        private async Task SplitError(string message, HttpContext context)
        {
            string[] parts = message.Split('|');

            if (parts.Length >= 2)
            {
                string errorMessage = parts[0].Substring(parts[0].LastIndexOf(":") + 1).Trim();
                int errorCode = int.Parse(parts[1].Trim() ?? "500");

                string result = await CreateProblemDetails(httpContext: context, statusCode: errorCode, detail: errorMessage);
                await context.Response.WriteAsync(result);
            }
            else
            {
                string result = await CreateProblemDetails(httpContext: context, statusCode: StatusCodes.Status500InternalServerError, detail: message);
                await context.Response.WriteAsync(result);
            }
        }
        public async Task CreateLog(HttpContext context, int duration)
        {

            // Ghi lại thông tin trước khi xử lý yêu cầu
            var log = new Logs
            {
                HttpMethod = context.Request.Method,
                Url = context.Request.Path,
                ClientIpAddress = context.Connection.RemoteIpAddress.ToString(),
                Time = DateTime.Now,
                Duration = duration,
                BrowserInfo = context.Request.Headers["User-Agent"],
                CorrelationId = context.TraceIdentifier,
                UserName = context.User.FindFirst("name")?.Value,
                StatusCode = context.Response.StatusCode
            };



            // Kiểm tra nếu URL chứa "/api/log" thì không lưu vào database
            if (!log.Url.Contains("/api/log", StringComparison.OrdinalIgnoreCase))
            {
                // Lưu log vào database
                await LogToDatabase(log);
            }

        }

        private async Task LogToDatabase(Logs log)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Contexts.AppDbContext>();
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
        public Task<string> CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            statusCode ??= 500;
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode.Value;

            var result = JsonConvert.SerializeObject(problemDetails);

            return Task.FromResult(result);
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            problemDetails.Instance = httpContext.Request.GetEncodedPathAndQuery();

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }
        }

    }

}
