using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductImage;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductImageService
    {
        public Task<PageResult<ProductImageDTO>> GetAllProductImage(Pagination pagination, int productID);
        public Task<ResponseObject<ProductImageDTO>> GetProductImageByID(int productImageID);
        public Task<ResponseObject<GetUpdateProductImageDTO>> GetUpdateProductImageByID(int productImageID);
        public Task<Response> CreateProductImage(CreateProductImageRequest request);
        public Task<Response> UpdateProductImage(int productImageID, UpdateProductImageRequest request);
        public Task<Response> RemoveProductImage(int productImageID);
    }
}
