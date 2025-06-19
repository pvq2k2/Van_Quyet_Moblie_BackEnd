using Application.Auth.Dto;

namespace Application.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> Login(AuthLoginDto request);
        Task<AuthDto> LoginWithGoogle(string idToken);
        Task<AuthDto> RefreshToken(string refreshToken);
        Task Logout(string refreshToken);
        Task ChangePassword(int userId, AuthChangePasswordDto request);
        Task<AuthUserDto?> GetCurrentUserInfo(int userId);
        Task Register(AuthRegiterDto request);
        Task ResendOtp(string email);
        Task VerifyEmailOtp(AuthVerifyEmailOtpDto request);
        Task ForgotPassword(string email);
        Task ResetPassword(AuthResetPasswordDto request);
    }
}
