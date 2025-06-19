using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Handle.Request.UserRequest;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iUserService;

        public UserController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        [HttpPost("get-all-user")]
        public async Task<IActionResult> GetAllUser(Pagination pagination)
        {
            return Ok(await _iUserService.GetAllUser(pagination));
        }

        [HttpGet("get-update-user-by-id/{userID}")]
        public async Task<IActionResult> GetUpdateUserByID([FromRoute] int userID)
        {
            return Ok(await _iUserService.GetUpdateUserByID(userID));
        }

        [HttpPut("update-user/{userID}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int userID, UpdateUserRequest request)
        {
            return Ok(await _iUserService.UpdateUser(userID, request));
        }
    }
}
