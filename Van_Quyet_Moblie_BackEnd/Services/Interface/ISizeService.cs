using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SizeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface ISizeService
    {
        public Task<PageResult<SizeDTO>> GetAllSize(Pagination pagination);
        public Task<Response> CreateSize(CreateSizeRequest request);
        public Task<ResponseObject<SizeDTO>> GetSizeByID(int sizeID);
        public Task<Response> UpdateSize(int sizeID, UpdateSizeRequest request);
    }
}
