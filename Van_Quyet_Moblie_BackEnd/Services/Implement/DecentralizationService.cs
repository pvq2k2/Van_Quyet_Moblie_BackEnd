using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.DecentralizationRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class DecentralizationService : IDecentralizationService
    {
        private readonly DecentralizationConverter _decentralizationConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<DecentralizationDTO> _response;
        public DecentralizationService(AppDbContext dbContext, ResponseObject<DecentralizationDTO> response)
        {
            _decentralizationConverter = new DecentralizationConverter();
            _dbContext = dbContext;
            _response = response;
        }
        public async Task<ResponseObject<DecentralizationDTO>> CreateDecentralization(CreateDecentralizationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !", null!);
            }

            var decentralization = new Decentralization { 
                Name = request.Name,
            };

            await _dbContext.Decentralization.AddAsync(decentralization);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm quyền thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }

        public async Task<PageResult<DecentralizationDTO>> GetAllDecentralization(Pagination pagination)
        {
            var query = _dbContext.Decentralization.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Decentralization>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<DecentralizationDTO>(pagination, _decentralizationConverter.ListEntityDecentralizationToDTO(result.ToList()));
        }

        public async Task<ResponseObject<DecentralizationDTO>> GetDecentralizationByID(int decentralizationID)
        {
            var decentralization = await _dbContext.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            if (decentralization == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Quyền không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }

        public async Task<ResponseObject<string>> RemoveDecentralization(int decentralizationID)
        {
            var response = new ResponseObject<string>();
            var decentralization = await _dbContext.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            if (decentralization == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Quyền không tồn tại !", null!);
            }

            _dbContext.Decentralization.Remove(decentralization);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Xóa quyền thành công !", null!);
        }

        public async Task<ResponseObject<DecentralizationDTO>> UpdateDecentralization(int decentralizationID, UpdateDecentralizationRequest request)
        {
            var decentralization = await _dbContext.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            if (decentralization == null)
            {
                return _response.ResponseError(StatusCodes.Status404NotFound, "Quyền không tồn tại !", null!);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !", null!);
            }

            decentralization.Name = request.Name;
            decentralization.UpdatedAt = DateTime.Now;

            _dbContext.Decentralization.Update(decentralization);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật quyền thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }
    }
}
