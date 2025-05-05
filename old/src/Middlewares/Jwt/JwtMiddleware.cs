using Libs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Middlewares.ErrorHandling;

namespace Middlewares.Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtUtils _jwtUtils;

        public JwtMiddleware(RequestDelegate next, IJwtUtils jwtUtils)
        {
            _next = next;
            _jwtUtils = jwtUtils;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

            if (endpoint != null)
            {
                var authorizeAttribute = endpoint.Metadata.OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().FirstOrDefault();

                if (authorizeAttribute != null)
                {
                    var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (string.IsNullOrEmpty(authorizationHeader))
                    {
                        throw new CustomException(StatusCodes.Status400BadRequest, "Không có token!");
                    }

                    if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new CustomException(StatusCodes.Status400BadRequest, "Không đúng dạng 'Bearer token'!");
                    }

                    var jwtToken = authorizationHeader["Bearer ".Length..].Trim();

                    // Xác thực token
                    var principal = _jwtUtils.ValidateToken(jwtToken) ?? throw new CustomException(StatusCodes.Status401Unauthorized, "Xác thực không hợp lệ !");
                }
            }
            await _next(context);
        }
    }
}
