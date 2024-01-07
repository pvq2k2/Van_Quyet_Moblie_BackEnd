using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ColorRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IColorService
    {
        public Task<PageResult<ColorDTO>> GetAllColor(Pagination pagination);
        public Task<Response> CreateColor(CreateColorRequest request);
        public Task<ResponseObject<ColorDTO>> GetColorByID(int colorID);
        public Task<Response> UpdateColor(int colorID, UpdateColorRequest request);
    }
}
