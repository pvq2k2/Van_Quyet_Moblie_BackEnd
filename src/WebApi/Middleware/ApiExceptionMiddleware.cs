using Application.Common.ExceptionHandling.Exceptions;
using log4net;
using System.Net;
using System.Text.Json;
using WebApi.Common.ApiResult;

namespace WebApi.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetLogger(typeof(ApiExceptionMiddleware)); // Khởi tạo logger log4net
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Tiếp tục luồng bình thường
            }
            catch (Exception ex)
            {
                _logger.Error("Đã xảy ra lỗi trong ứng dụng", ex);  // Ghi log chi tiết về lỗi
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message = exception.Message;
            object? errorDetails = null;

            switch (exception)
            {
                case ValidationException validationEx:
                    statusCode = validationEx.StatusCode;
                    message = validationEx.Message;
                    errorDetails = validationEx.Errors;
                    break;

                case FriendlyException friendlyEx:
                    statusCode = friendlyEx.StatusCode;
                    message = friendlyEx.Message;
                    errorDetails = friendlyEx.Details;
                    break;

                case UnauthorizedAccessException unauthorizedAccessEx:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = unauthorizedAccessEx.Message ?? "Bạn chưa đăng nhập hoặc quyền truy cập không hợp lệ.";
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Tạo ApiResult cho lỗi
            var apiResult = ApiResult<object>.FailureResult(message, statusCode, errorDetails);
            var result = JsonSerializer.Serialize(apiResult);

            return context.Response.WriteAsync(result);
        }
    }
}
