using API.Authorization;
using API.Permissions;
using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Requests.Permission;

namespace API.Controllers
{
    [Route("api/permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("create-permission")]
        [HasPermission(APIPermissions.Permission.Create)]
        public async Task<IActionResult> CreatePermission(CreateUpdatePermissionRequest request)
        {
            return Ok(await _permissionService.CreatePermission(request));
        }

        [HttpPost("get-all-permission")]
        [HasPermission(APIPermissions.Permission.View)]
        public async Task<IActionResult> GetAllPermission(FilterPermissionRequest filter)
        {
            return Ok(await _permissionService.GetAllPermission(filter));
        }

        [HttpGet("get-permission-by-id/{permissionId}")]
        [HasPermission(APIPermissions.Permission.View)]
        public async Task<IActionResult> GetPermissionById(int permissionId)
        {

            return Ok(await _permissionService.GetPermissionById(permissionId));
        }

        [HttpPut("update-permission/{permissionId}")]
        [HasPermission(APIPermissions.Permission.Update)]
        public async Task<IActionResult> UpdatePermission(int permissionId, CreateUpdatePermissionRequest request)
        {
            return Ok(await _permissionService.UpdatePermission(permissionId, request));        
        }

        [HttpDelete("delete-permission/{permissionId}")]
        [HasPermission(APIPermissions.Permission.Delete)]
        public async Task<IActionResult> DeletePermission(int permissionId)
        {
            return Ok(await _permissionService.DeletePermission(permissionId));
        }
    }
}
