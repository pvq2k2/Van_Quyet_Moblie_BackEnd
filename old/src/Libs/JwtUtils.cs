using DTOs.Role;
using Microsoft.IdentityModel.Tokens;
using Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Libs
{
    public interface IJwtUtils
    {
        public string CreateAccessToken(Users user, List<RoleTokenDTO> listRoleTokenDTO);
        public ClaimsPrincipal? ValidateToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfigurationUtils _configurationUtils;

        public JwtUtils(IConfigurationUtils configurationUtils)
        {
            _configurationUtils = configurationUtils;
        }

        public string CreateAccessToken(Users users, List<RoleTokenDTO> listRoleTokenDTO)
        {
            // Khởi tạo các claims cho người dùng
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sid, users.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, users.FullName!),
                new(JwtRegisteredClaimNames.Email, users.Email!),
            };

            // Chuyển đổi danh sách các RoleTokenDTO thành chuỗi JSON
            var rolesJson = JsonConvert.SerializeObject(listRoleTokenDTO.Select(role => new
            {
                id = role.Id,
                name = role.Name
            }).ToList());

            // Thêm claim cho vai trò
            claims.Add(new Claim(ClaimTypes.Role, rolesJson));

            // Tạo SymmetricSecurityKey từ secret key trong cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configurationUtils.GetConfiguration("AppSettings:JWT:AccessTokenSecret")));

            // Thiết lập SigningCredentials cho việc ký token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo SecurityTokenDescriptor mô tả các thuộc tính của token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(10), // Thời gian hết hạn của token
                SigningCredentials = creds,
                Issuer = _configurationUtils.GetConfiguration("AppSettings:JWT:ValidIssuer"), // Tên người phát hành token
                Audience = _configurationUtils.GetConfiguration("AppSettings:JWT:ValidAudience") // Đối tượng người nhận token
            };

            // Sử dụng JwtSecurityTokenHandler để tạo token từ SecurityTokenDescriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Trả về chuỗi token
            return tokenHandler.WriteToken(token);
        }



        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configurationUtils.GetConfiguration("AppSettings:JWT:AccessTokenSecret"));

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _configurationUtils.GetConfiguration("AppSettings:JWT:ValidIssuer"), // Tên người phát hành token
                    ValidAudience = _configurationUtils.GetConfiguration("AppSettings:JWT:ValidAudience"), // Đối tượng người nhận token
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
