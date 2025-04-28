using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductReview;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductReviewService
    {
        public Task<PageResult<ProductReviewDTO>> GetAllProductReview(int productID, Pagination pagination);
        public Task<PageResult<GetProductReviewToViewDTO>> GetProductReviewToView(int productID, Pagination pagination);
        public Task<ResponseObject<ProductReviewDTO>> GetProductReviewByID(int productReviewID);
        public Task<Response> CreateProductReview(CreateProductReviewRequest request);
        public Task<Response> UpdateProductReview(int productReviewID, UpdateProductReviewRequest request);
        public Task<Response> RemoveProductReview(int productReviewID);
    }
}
