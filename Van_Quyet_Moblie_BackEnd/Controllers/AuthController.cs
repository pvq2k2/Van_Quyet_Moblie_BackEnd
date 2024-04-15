using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;

        public AuthController(IAuthService iAuthService) {
            _iAuthService = iAuthService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            return Ok(await _iAuthService.Register(request));
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            return Ok(await _iAuthService.VerifyEmail(token));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            return Ok(await _iAuthService.Login(request));
        }

        [HttpPost("re-new-token")]
        public async Task<IActionResult> ReNewToken(string refreshToken)
        {
            return Ok(await _iAuthService.ReNewToken(refreshToken));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            return Ok(await _iAuthService.ForgotPassword(email));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            return Ok(await _iAuthService.ResetPassword(request));
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            return Ok(await _iAuthService.ChangePassword(request));
        }

        [HttpPost("change-information")]
        public async Task<IActionResult> ChangeInformation([FromForm]ChangeInformationRequest request)
        {
            return Ok(await _iAuthService.ChangeInformation(request));
        }
    }
}
