using API.Authorization;
using API.Permissions;
using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Requests.User;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("get-all-user")]
        [HasPermission(APIPermissions.User.View)]
        public async Task<IActionResult> GetAllUser(FilterUserRequest filter) {
            return Ok(await _userService.GetAllUser(filter));
        }

        [HttpGet("get-user-by-id/{userId}")]
        [HasPermission(APIPermissions.User.View)]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            return Ok(await _userService.GetUserById(userId));
        }

        [HttpGet("get-all-data-user-by-id/{userId}")]
        [HasPermission(APIPermissions.User.View)]
        public async Task<IActionResult> GetFullDataUserById([FromRoute] int userId)
        {
            return Ok(await _userService.GetFullDataUserById(userId));
        }

        [HttpPost("create-user")]
        [HasPermission(APIPermissions.User.Create)]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            return Ok(await _userService.CreateUser(request));
        }

        [HttpPut("update-user/{userId}")]
        [HasPermission(APIPermissions.User.Update)]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserRequest request)
        {
            return Ok(await _userService.UpdateUser(userId, request));
        }

        [HttpDelete("detete-user/{userId}")]
        [HasPermission(APIPermissions.User.Delete)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            return Ok(await _userService.DeleteUser(userId));
        }
    }
}
