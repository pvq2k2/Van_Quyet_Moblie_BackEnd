using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Common.JwtToken
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string AccessToken, string RefreshToken) GenerateTokens(User user, List<string> permissions)
        {
            // Bước 1: Khởi tạo claims cơ bản
            var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Email, user.Email),
        new("username", user.UserName),
    };

            // Bước 2: Thêm claims permission
            claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

            // Bước 3: Chuẩn bị key và credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AccessTokenSecret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Bước 4: Cấu hình token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            // Bước 5: Tạo refresh token đơn giản (có thể thay đổi theo nhu cầu)
            var refreshToken = Guid.NewGuid().ToString();

            // Bước 6: Trả kết quả
            return (accessToken, refreshToken);
        }
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JWT:AccessTokenSecret"]!);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _configuration["JWT:ValidIssuer"], // Tên người phát hành token
                    ValidAudience = _configuration["JWT:ValidAudience"], // Đối tượng người nhận token
                    RoleClaimType = ClaimTypes.Role,
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return principal; // Trả về ClaimsPrincipal nếu xác thực thành công
            }
            catch (SecurityTokenException)
            {
                return null; // Trả về null nếu token không hợp lệ
            }
        }
    }
}
