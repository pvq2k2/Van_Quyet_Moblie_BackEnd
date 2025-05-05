using AutoMapper;
using Business.Interface;
using Contexts;
using Dapper;
using DTOs.Role;
using Microsoft.AspNetCore.Http;
using Middlewares.ErrorHandling;
using Models;
using Requests;
using Requests.Role;
using Responses;
using System.Data;
using System.Text.Json;

namespace Business.Implement
{
    public class RoleService : IRoleService
    {
        private readonly IDapperContext _dapperContext;
        private readonly IMapper _mapper;

        public RoleService(IDapperContext dapperContext, IMapper mapper)
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
        }

        private static async Task<Roles> IsRoleExists(int roleId, IDbConnection connection)
        {
            var stdName = "Role_GetById";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", roleId);

            var roleExists = await connection.QueryFirstOrDefaultAsync<Roles>(
                stdName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return roleExists!;
        }

        public async Task<ResponseText> CreateRole(CreateUpdateRoleRequest request)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                request.Name = request.Name!.ToLower();

                var stdName = "Role_Create";
                var roleJson = JsonSerializer.Serialize(request);
                var parameters = new DynamicParameters();
                parameters.Add("@RoleData", roleJson);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);
                return ResponseText.ResponseSuccess("Thêm vai trò thành công !");
            }
        }

        public async Task<ResponseText> DeleteRole(int roleId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var role = await IsRoleExists(roleId, connection);

                if (role == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Vai trò không tồn tại !");
                }

                var stdName = "Role_Delete";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                return ResponseText.ResponseSuccess("Xóa vai trò thành công !");

            }
        }
        public async Task<PageResult<RoleDTO>> GetAllRole(FilterRoleRequest filter)
        {
            if (filter.PageNumber < 1)
            {
                filter.PageNumber = 1;
            }

            if (filter.PageSize <= 0)
            {
                filter.PageSize = 10;
            }
            using (var connection = _dapperContext.CreateConnection())
            {
                var stdName = "Role_GetAll";
                var parameters = new DynamicParameters();
                parameters.Add("@FilterRoleData", JsonSerializer.Serialize(filter));
                parameters.Add("@PageNumber", filter.PageNumber);
                parameters.Add("@PageSize", filter.PageSize);

                var roles = await connection.QueryAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                var totalCount = roles.FirstOrDefault()?.TotalRecords;

                var pagination = new Pagination() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, TotalCount = totalCount ?? 0 };
                var roleDtoList = _mapper.Map<List<RoleDTO>>(roles);

                return new PageResult<RoleDTO>(pagination, roleDtoList);
            }
        }


        public async Task<ResponseObject<RoleDTO>> GetRoleById(int roleId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var role = await IsRoleExists(roleId, connection);

                if (role == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Vai trò không tồn tại !");
                }

                var roleDTO = _mapper.Map<RoleDTO>(role);

                return ResponseObject<RoleDTO>.ResponseSuccess("Thành công", roleDTO);
            }
        }

        public async Task<ResponseObject<GetAllDataRoleDTO>> GetFullDataRoleById(int roleId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();

                var stdName = "Role_GetFullDataById";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId);

                var roleDictionary = new Dictionary<int, Roles>();

                var roleResponse = (await connection.QueryAsync<Roles, PermissionRole, Permissions, RoleMenu, Menus, Roles>(
                    stdName,
                    (role, permissionRole, permissions, roleMenu, menu) =>
                    {
                        if (!roleDictionary.TryGetValue(role.Id, out var existingRole))
                        {
                            existingRole = role;
                            existingRole.ListPermissionRole = new List<PermissionRole>();
                            existingRole.ListRoleMenu = new List<RoleMenu>();
                            roleDictionary.Add(existingRole.Id, existingRole);
                        }

                        if (permissionRole != null)
                        {
                            // Kiểm tra xem permissionRole đã tồn tại trong danh sách chưa
                            if (!existingRole.ListPermissionRole!.Any(pr => pr.Id == permissionRole.Id))
                            {
                                permissionRole.Permission = permissions;
                                existingRole.ListPermissionRole!.Add(permissionRole);
                            }
                        }

                        if (roleMenu != null)
                        {
                            // Kiểm tra xem roleMenu đã tồn tại trong danh sách chưa
                            if (!existingRole.ListRoleMenu!.Any(rm => rm.Id == roleMenu.Id))
                            {
                                roleMenu.Menu = menu;
                                existingRole.ListRoleMenu!.Add(roleMenu);
                            }
                        }


                        return existingRole;
                    },
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Id"
                )).Distinct().ToList();

                var roleData = roleDictionary.Values.FirstOrDefault();
                if (roleData == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Vai trò không tồn tại !");
                }

                var getAllDataRoleDTO = _mapper.Map<GetAllDataRoleDTO>(roleData);

                return ResponseObject<GetAllDataRoleDTO>.ResponseSuccess("Thành công", getAllDataRoleDTO);
            }
        }



        public async Task<ResponseText> UpdateRole(int roleId, CreateUpdateRoleRequest request)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var role = await IsRoleExists(roleId, connection);
                if (role == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Vai trò không tồn tại !");
                }

                var stdName = "Role_Update";
                var roleJson = JsonSerializer.Serialize(request);
                var parameters = new DynamicParameters();
                parameters.Add("@RoleData", roleJson);
                parameters.Add("@RoleId", roleId);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);


                return ResponseText.ResponseSuccess("Cập nhật quyền thành công !");
            }
        }
    }
}
