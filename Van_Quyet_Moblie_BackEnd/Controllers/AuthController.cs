using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
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

        [HttpPost("get-all-account")]
        public async Task<IActionResult> GetAllAccount(Pagination pagination)
        {
            return Ok(await _iAuthService.GetAllAccount(pagination));
        }

        [HttpGet("get-account-by-id/{accountID}")]
        public async Task<IActionResult> GetAccountByID([FromRoute] int accountID)
        {
            return Ok(await _iAuthService.GetAccountByID(accountID));
        }

        [HttpPost("change-information")]
        public async Task<IActionResult> ChangeInformation([FromForm]ChangeInformationRequest request)
        {
            return Ok(await _iAuthService.ChangeInformation(request));
        }

        [HttpPatch("change-status")]
        public async Task<IActionResult> ChangeStatus(int accountID, int status)
        {
            return Ok(await _iAuthService.ChangeStatus(accountID, status));
        }
    }
}
