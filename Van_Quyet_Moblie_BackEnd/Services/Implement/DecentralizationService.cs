using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.DecentralizationRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class DecentralizationService : BaseService, IDecentralizationService
    {
        private readonly ResponseObject<DecentralizationDTO> _response;
        public DecentralizationService(ResponseObject<DecentralizationDTO> response)
        {
            _response = response;
        }
        public async Task<ResponseObject<DecentralizationDTO>> CreateDecentralization(CreateDecentralizationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AuthorityName))
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !", null!);
            }

            var decentralization = new Decentralization { 
                AuthorityName = request.AuthorityName,
            };

            await _context.Decentralization.AddAsync(decentralization);
            await _context.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm quyền thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }

        public async Task<PageResult<DecentralizationDTO>> GetAllDecentralization(Pagination pagination)
        {
            var query = _context.Decentralization.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Decentralization>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<DecentralizationDTO>(pagination, _decentralizationConverter.ListEntityDecentralizationToDTO(result.ToList()));
        }

        public async Task<ResponseObject<DecentralizationDTO>> GetDecentralizationByID(int decentralizationID)
        {
            var decentralization = await _context.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            if (decentralization == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Quyền không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }

        public async Task<ResponseObject<string>> RemoveDecentralization(int decentralizationID)
        {
            var response = new ResponseObject<string>();
            var decentralization = await _context.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            if (decentralization == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Quyền không tồn tại !", null!);
            }

            _context.Decentralization.Remove(decentralization);
            await _context.SaveChangesAsync();

            return response.ResponseSuccess("Xóa quyền thành công !", null!);
        }

        public async Task<ResponseObject<DecentralizationDTO>> UpdateDecentralization(int decentralizationID, UpdateDecentralizationRequest request)
        {
            var decentralization = await _context.Decentralization.FirstOrDefaultAsync(x => x.ID == decentralizationID);
            if (decentralization == null)
            {
                return _response.ResponseError(StatusCodes.Status404NotFound, "Quyền không tồn tại !", null!);
            }

            if (string.IsNullOrWhiteSpace(request.AuthorityName))
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !", null!);
            }

            decentralization.AuthorityName = request.AuthorityName;
            decentralization.UpdatedAt = DateTime.Now;

            _context.Decentralization.Update(decentralization);
            await _context.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật quyền thành công !", _decentralizationConverter.EntityDecentralizationToDTO(decentralization));
        }
    }
}
