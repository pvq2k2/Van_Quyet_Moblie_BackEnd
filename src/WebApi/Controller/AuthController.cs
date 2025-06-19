using Domain.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Auth;
using Application.Auth.Dto;
using WebApi.Common.ApiResult;
using System.Security.Claims;

namespace WebApi.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto request)
        {
            var result = await _authService.Login(request);
            return Ok(ApiResult<AuthDto>.SuccessResult(result, "Đăng nhập thành công."));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string idToken)
        {
            var result = await _authService.LoginWithGoogle(idToken);
            return Ok(ApiResult<AuthDto>.SuccessResult(result, "Đăng nhập thành công."));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);
            return Ok(ApiResult<AuthDto>.SuccessResult(result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _authService.Logout(refreshToken);
            return Ok(ApiResult<string>.SuccessResult("Đăng xuất thành công."));
        }

        [Authorize(Policy = AppPermissions.User.Get)]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Không xác định được người dùng.");

            var user = await _authService.GetCurrentUserInfo(userId);
            if (user == null)
                throw new KeyNotFoundException("Không tìm thấy người dùng.");

            return Ok(ApiResult<AuthUserDto>.SuccessResult(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegiterDto request)
        {
            await _authService.Register(request);
            return Ok(ApiResult<string>.SuccessResult("Đăng ký thành công."));
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] string email)
        {
            await _authService.ResendOtp(email);
            return Ok(ApiResult<string>.SuccessResult("Làm mới Otp thành công."));
        }

        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOtp([FromBody] AuthVerifyEmailOtpDto request)
        {
            await _authService.VerifyEmailOtp(request);
            return Ok(ApiResult<string>.SuccessResult("Xác thực thành công."));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            await _authService.ForgotPassword(email);
            return Ok(ApiResult<string>.SuccessResult("Vui lòng kiểm tra hộp thư của bạn để đặt lại mật khẩu."));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] AuthResetPasswordDto request)
        {
            await _authService.ResetPassword(request);
            return Ok(ApiResult<string>.SuccessResult("Đổi mật khẩu thành công."));
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] AuthChangePasswordDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Không xác định được người dùng.");

            await _authService.ChangePassword(userId, request);
            return Ok(ApiResult<string>.SuccessResult("Đổi mật khẩu thành công."));
        }
    }
}
