using DTOs.Auth;
using Requests.Auth;
using Responses;

namespace Business.Interface
{
    public interface IAuthService
    {
        public Task<ResponseObject<AuthDTO>> Login(LoginRequest loginRequest);
        public Task<ResponseText> Register(RegisterRequest registerRequest);
        public Task<ResponseObject<AuthDTO>> ReNewToken(string refreshToken);
    }
}
