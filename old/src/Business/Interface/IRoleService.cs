using DTOs.Role;
using Requests.Role;
using Responses;

namespace Business.Interface
{
    public interface IRoleService
    {
        public Task<PageResult<RoleDTO>> GetAllRole(FilterRoleRequest filter);
        public Task<ResponseObject<RoleDTO>> GetRoleById(int roleId);
        public Task<ResponseObject<GetAllDataRoleDTO>> GetFullDataRoleById(int roleId);
        public Task<ResponseText> CreateRole(CreateUpdateRoleRequest request);
        public Task<ResponseText> UpdateRole(int roleId, CreateUpdateRoleRequest request);
        public Task<ResponseText> DeleteRole(int roleId);
    }
}
