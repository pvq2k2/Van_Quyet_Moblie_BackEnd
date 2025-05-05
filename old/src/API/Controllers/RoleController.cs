using API.Authorization;
using API.Permissions;
using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Requests.Role;

namespace API.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("get-all-role")]
        [HasPermission(APIPermissions.Role.View)]
        public async Task<IActionResult> GetAllRole(FilterRoleRequest filter)
        {
            return Ok(await _roleService.GetAllRole(filter));
        }

        [HttpGet("get-role-by-id/{roleId}")]
        [HasPermission(APIPermissions.Role.View)]
        public async Task<IActionResult> GetRoleById([FromRoute] int roleId)
        {
            return Ok(await _roleService.GetRoleById(roleId));
        }

        [HttpGet("get-full-data-role-by-id/{roleId}")]
        // [HasPermission(APIPermissions.Role.View)]
        public async Task<IActionResult> GetFullDataRoleById([FromRoute] int roleId)
        {
            return Ok(await _roleService.GetFullDataRoleById(roleId));
        }

        [HttpPost("create-role")]
        [HasPermission(APIPermissions.Role.Create)]
        public async Task<IActionResult> CreateRole(CreateUpdateRoleRequest request)
        {
            return Ok(await _roleService.CreateRole(request));
        }

        [HttpPut("update-role/{roleId}")]
        [HasPermission(APIPermissions.Role.Update)]
        public async Task<IActionResult> UpdateRole([FromRoute] int roleId, CreateUpdateRoleRequest request)
        {
            return Ok(await _roleService.UpdateRole(roleId, request));
        }

        [HttpDelete("delete-role/{roleId}")]
        [HasPermission(APIPermissions.Role.Delete)]
        public async Task<IActionResult> DeleteRole([FromRoute] int roleId)
        {
            return Ok(await _roleService.DeleteRole(roleId));
        }
    }
}
