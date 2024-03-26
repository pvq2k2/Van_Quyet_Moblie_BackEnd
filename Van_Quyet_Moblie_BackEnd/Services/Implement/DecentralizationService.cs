using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.DecentralizationRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Services.Interface;
using Van_Quyet_Moblie_BackEnd.Middleware;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class DecentralizationService : IDecentralizationService
    {
        private readonly DecentralizationConverter _decentralizationConverter;
        private readonly TokenHelper _tokenHelper;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<DecentralizationDTO> _responseDecentralization;
        private readonly Response _response;
        public DecentralizationService(AppDbContext dbContext, ResponseObject<DecentralizationDTO> responseDecentralization, Response response, TokenHelper tokenHelper)
        {
            _decentralizationConverter = new DecentralizationConverter();
            _dbContext = dbContext;
            _responseDecentralization = responseDecentralization;
            _response = response;
            _tokenHelper = tokenHelper;
        }
        #region Validate
        private async Task<Decentralization> IsDecentralizationExist(int decentralizationID)
        {
            var decentralization = await _dbContext.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            return decentralization ?? throw new CustomException(StatusCodes.Status404NotFound, "Quyền không tồn tại !");
        }
        private static void ValidateDecentralization(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên quyền không được để trống !");
            }
        }
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
        }
        #endregion
        public async Task<Response> CreateDecentralization(CreateDecentralizationRequest request)
        {
            IsAdmin();
            ValidateDecentralization(request.Name!);

            var decentralization = new Decentralization { 
                Name = request.Name,
            };

            await _dbContext.Decentralization.AddAsync(decentralization);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm quyền thành công !");
        }

        public async Task<PageResult<DecentralizationDTO>> GetAllDecentralization(Pagination pagination)
        {
            IsAdmin();
            var query = _dbContext.Decentralization.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Decentralization>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<DecentralizationDTO>(pagination, _decentralizationConverter.ListEntityDecentralizationToDTO(result.ToList()));
        }

        public async Task<ResponseObject<DecentralizationDTO>> GetDecentralizationByID(int decentralizationID)
        {
            var decentralization = await IsDecentralizationExist(decentralizationID);
            return _responseDecentralization.ResponseSuccess("Thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }

        public async Task<Response> RemoveDecentralization(int decentralizationID)
        {
            IsAdmin();
            var decentralization = await IsDecentralizationExist(decentralizationID);

            _dbContext.Decentralization.Remove(decentralization);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Xóa quyền thành công !");
        }

        public async Task<Response> UpdateDecentralization(int decentralizationID, UpdateDecentralizationRequest request)
        {
            IsAdmin();
            ValidateDecentralization(request.Name!);
            var decentralization = await IsDecentralizationExist(decentralizationID);

            decentralization.Name = request.Name;
            decentralization.UpdatedAt = DateTime.Now;

            _dbContext.Decentralization.Update(decentralization);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật quyền thành công !");
        }
    }
}
