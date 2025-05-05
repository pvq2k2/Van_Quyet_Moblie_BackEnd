using Dapper;
using System.Data;
using API.Authorization;
using Contexts;
using DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Middlewares.ErrorHandling;
using Newtonsoft.Json;
using System.Security.Claims;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IDapperContext _dapperContext;

    public PermissionHandler(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    private async Task<bool> CheckRolePermission(int roleId, string permissionName)
    {
        using (var connection = _dapperContext.CreateConnection())
        {
            connection.Open();
            var storedProcedure = "Role_CheckRolePermission";
            var parameters = new DynamicParameters();
            parameters.Add("@roleId", roleId);
            parameters.Add("@permissionName", permissionName);

            // Truy vấn stored procedure và lấy kết quả trả về
            var hasPermission = await connection.ExecuteScalarAsync<int>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return hasPermission == 1; // Trả về true nếu có quyền, false nếu không
        }
    }


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // Kiểm tra kiểu của context.Resource
        if (context.Resource is HttpContext httpContext)
        {
            // Kiểm tra xem người dùng đã xác thực hay chưa
            if (!context.User.Identity!.IsAuthenticated)
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Người dùng chưa đăng nhập.");
            }

            // Lấy endpoint hiện tại từ HttpContext
            var endpoint = httpContext.GetEndpoint();

            // Lấy HasPermissionAttribute từ metadata của endpoint
            var hasPermissionAttribute = endpoint?.Metadata.GetMetadata<HasPermissionAttribute>();

            if (hasPermissionAttribute == null)
            {
                return; // Không có thuộc tính quyền thì bỏ qua
            }

            var requiredPermission = hasPermissionAttribute.Permission;

            // Lấy thông tin vai trò từ token
            var rolesClaim = context.User.FindFirst(ClaimTypes.Role); // Thay ClaimTypes.Role bằng tên claim bạn đã định nghĩa
            List<RoleTokenDTO> listRoleTokenDTO = new List<RoleTokenDTO>();

            if (rolesClaim != null)
            {
                // Chuyển đổi JSON về danh sách RoleTokenDTO
                listRoleTokenDTO = JsonConvert.DeserializeObject<List<RoleTokenDTO>>(rolesClaim.Value) ?? new List<RoleTokenDTO>();
            }

            bool isPermission = false;

            // Kiểm tra từng vai trò của người dùng xem có quyền không
            foreach (var roleToken in listRoleTokenDTO)
            {
                var hasPermission = await CheckRolePermission(roleToken.Id, requiredPermission);

                if (hasPermission)
                {
                    isPermission = true;
                    break;
                }
            }

            if (isPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                // Ném ngoại lệ nếu người dùng không có quyền
                throw new CustomException(StatusCodes.Status403Forbidden, "Bạn không có quyền truy cập!");
            }
        }
    }

}
