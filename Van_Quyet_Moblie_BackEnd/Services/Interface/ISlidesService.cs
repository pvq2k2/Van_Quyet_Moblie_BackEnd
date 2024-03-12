using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface ISlidesService
    {
        public Task<PageResult<SlidesDTO>> GetAllSlides(Pagination pagination);
        public Task<ResponseObject<List<SlidesDTO>>> GetActiveSlides();
        public Task<ResponseObject<SlidesDTO>> GetSlidesByID(int slidesID);
        public Task<ResponseObject<SlidesDTO>> CreateSlides(CreateSlidesRequest request);
        public Task<ResponseObject<SlidesDTO>> UpdateSlides(int slidesID, UpdateSlidesRequest request);
        public Task<ResponseObject<string>> RemoveSlides(int slidesID);
    }
}
