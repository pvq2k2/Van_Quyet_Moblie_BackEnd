using AutoMapper;
using Business.Interface;
using Contexts;
using Dapper;
using System.Text.Json;
using Middlewares.ErrorHandling;
using Models;
using Requests;
using Requests.Permission;
using Responses;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace Business.Implement
{
    public class PermissionService : IPermissionService
    {
        private readonly IDapperContext _dapperContext;
        private readonly IMapper _mapper;

        public PermissionService(IDapperContext dapperContext, IMapper mapper)
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
        }
        public async Task<ResponseText> CreatePermission(CreateUpdatePermissionRequest request)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                request.GroupName = request.Name.Split('.')[0];
                var stdName = "Permission_Create";
                var permissionJson = JsonSerializer.Serialize(request);
                var parameters = new DynamicParameters();
                parameters.Add("@PermissionData", permissionJson);
                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);
                return ResponseText.ResponseSuccess("Thêm quyền thành công !");
            }
        }

        public async Task<ResponseText> DeletePermission(int permissionId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var permission = await IsPermissionExists(permissionId, connection);

                if (permission == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Quyền không tồn tại !");
                }

                var stdName = "Permission_Delete";
                var parameters = new DynamicParameters();
                parameters.Add("@PermissionId", permissionId);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                return ResponseText.ResponseSuccess("Thành công");
            }
        }

        public async Task<PageResult<Permissions>> GetAllPermission(FilterPermissionRequest filter)
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
                var stdName = "Permission_GetAll";
                var parameters = new DynamicParameters();
                parameters.Add("@FilterPermissionData", JsonSerializer.Serialize(filter));
                parameters.Add("@PageNumber", filter.PageNumber);
                parameters.Add("@PageSize", filter.PageSize);

                var permissions = await connection.QueryAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                var totalCount = permissions.FirstOrDefault()?.TotalRecords;

                var permissionDtoList = _mapper.Map<List<Permissions>>(permissions);
                var pagination = new Pagination() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, TotalCount = totalCount ?? 0 };

                return new PageResult<Permissions>(pagination, permissionDtoList);
            }
        }

        public async Task<ResponseObject<Permissions>> GetPermissionById(int permissionId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var permission = await IsPermissionExists(permissionId, connection);

                if (permission == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Quyền không tồn tại !");
                }

                return ResponseObject<Permissions>.ResponseSuccess("Thành công", permission);
            }
        }

        public async Task<ResponseText> UpdatePermission(int permissionId, CreateUpdatePermissionRequest request)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var permission = await IsPermissionExists(permissionId, connection);

                if (permission == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Quyền không tồn tại !");
                }

                request.GroupName = request.Name.Split('.')[0];
                var stdName = "Permission_Update";
                var permissionData = JsonSerializer.Serialize(request);
                var parameters = new DynamicParameters();
                parameters.Add("@PermissionData", permissionData);
                parameters.Add("@PermissionId", permissionId);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                return ResponseText.ResponseSuccess("Cập nhật quyền thành công !");
            }
        }

        private static async Task<Permissions> IsPermissionExists(int permissionId, IDbConnection connection)
        {
            var stdName = "Permission_GetById";
            var parameters = new DynamicParameters();
            parameters.Add("@PermissionId", permissionId);

            var permissionExists = await connection.QueryFirstOrDefaultAsync<Permissions>(
                stdName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return permissionExists!;
        }
    }
}
