using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductReviewService
    {
        public Task<PageResult<ProductReviewDTO>> GetAllProductReview(Pagination pagination);
        public Task<ResponseObject<ProductReviewDTO>> GetProductReviewByID(int productReviewID);
        public Task<ResponseObject<ProductReviewDTO>> CreateProductReview(CreateProductReviewRequest request);
        public Task<ResponseObject<ProductReviewDTO>> UpdateProductReview(int productReviewID, UpdateProductReviewRequest request);
        public Task<ResponseObject<string>> RemoveProductReview(int productReviewID);
    }
}
