using System.Security.Claims;
using Domain.Entities;

namespace Application.Common.JwtToken
{
    public interface IJwtTokenService
    {
        (string AccessToken, string RefreshToken) GenerateTokens(User user, List<string> permissions);
        public ClaimsPrincipal? ValidateToken(string token);
    }
}
