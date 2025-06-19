using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Slides;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface ISlidesService
    {
        public Task<PageResult<SlidesDTO>> GetAllSlides(Pagination pagination);
        public Task<ResponseObject<List<GetActiveSlidesDTO>>> GetActiveSlides();
        public Task<ResponseObject<SlidesDTO>> GetSlidesByID(int slidesID);
        public Task<Response> CreateSlides(SlidesRequest request);
        public Task<Response> UpdateSlides(int slidesID, SlidesRequest request);
        public Task<Response> RemoveSlides(int slidesID);
    }
}
