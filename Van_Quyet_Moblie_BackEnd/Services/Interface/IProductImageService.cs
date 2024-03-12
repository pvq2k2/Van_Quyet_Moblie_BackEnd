using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductImageService
    {
        public Task<PageResult<ProductImageDTO>> GetAllProductImage(Pagination pagination);
        public Task<ResponseObject<ProductImageDTO>> GetProductImageByID(int productImageID);
        public Task<ResponseObject<ProductImageDTO>> CreateProductImage(CreateProductImageRequest request);
        public Task<ResponseObject<ProductImageDTO>> UpdateProductImage(int productImageID, UpdateProductImageRequest request);
        public Task<ResponseObject<string>> RemoveProductImage(int productImageID);
    }
}
