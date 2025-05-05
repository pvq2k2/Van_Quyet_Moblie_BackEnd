using Models;
using Requests.Permission;
using Responses;

namespace Business.Interface
{
    public interface IPermissionService
    {
        public Task<PageResult<Permissions>> GetAllPermission(FilterPermissionRequest filter);
        public Task<ResponseObject<Permissions>> GetPermissionById(int permissionId);
        public Task<ResponseText> CreatePermission(CreateUpdatePermissionRequest request);
        public Task<ResponseText> UpdatePermission(int permissionId, CreateUpdatePermissionRequest request);
        public Task<ResponseText> DeletePermission(int permissionId);
    }
}
