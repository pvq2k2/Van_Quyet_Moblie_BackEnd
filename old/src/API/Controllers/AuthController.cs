using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Requests.Auth;

namespace API.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            return Ok(await _authService.Register(registerRequest));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            return Ok(await _authService.Login(loginRequest));
        }

        [HttpGet("re-new-token")]
        public async Task<IActionResult> ReNewToken(string refreshToken)
        {
            return Ok(await _authService.ReNewToken(refreshToken));
        }
    }
}
