using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Van_Quyet_Moblie_BackEnd.Middleware;

namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsToken()
        {
            var authorizationHeader = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new CustomException(StatusCodes.Status401Unauthorized,"Không có token!");
            }

            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không đúng dạng 'Bearer token'!");
            }

            var jwtToken = authorizationHeader["Bearer ".Length..].Trim();

            if (!ValidateToken(jwtToken))
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Xác thực không hợp lệ !");
            }

            return true;
        }

        public int GetUserID()
        {
            var userIDClaim = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "ID");
            bool isUserID = int.TryParse(userIDClaim!.Value, out int userID);
            if (!isUserID) {
                throw new Exception("Không lấy được userID");
            }
            return userID;
        }

        public string GetRole()
        {
            var roleClaim = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            string role = roleClaim!.Value.ToString();
            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("Không lấy được role");
            }
            return role;
        }

        private bool ValidateToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:AccessTokenSecret").Value!);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
    }
}
