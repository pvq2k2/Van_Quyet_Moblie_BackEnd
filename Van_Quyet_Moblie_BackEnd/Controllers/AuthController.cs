using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;

        public AuthController(IAuthService iAuthService) {
            _iAuthService = iAuthService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            return Ok(await _iAuthService.Register(request));
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            return Ok(await _iAuthService.VerifyEmail(token));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            return Ok(await _iAuthService.Login(request));
        }

        [HttpPost("ReNewToken")]
        public IActionResult ReNewToken(string refreshToken)
        {
            return Ok(_iAuthService.ReNewToken(refreshToken));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            return Ok(await _iAuthService.ForgotPassword(email));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            return Ok(await _iAuthService.ResetPassword(request));
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            return Ok(await _iAuthService.ChangePassword(request));
        }

        [HttpPost("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount(Pagination pagination)
        {
            return Ok(await _iAuthService.GetAllAccount(pagination));
        }

        [HttpGet("GetAccountByID")]
        public async Task<IActionResult> GetAccountByID(int accountID)
        {
            return Ok(await _iAuthService.GetAccountByID(accountID));
        }

        [HttpPost("ChangeInformation")]
        public async Task<IActionResult> ChangeInformation([FromForm] ChangeInformationRequest request)
        {
            return Ok(await _iAuthService.ChangeInformation(request));
        }

        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int accountID, int status)
        {
            return Ok(await _iAuthService.ChangeStatus(accountID, status));
        }
    }
}
