using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductService
    {
        public Task<PageResult<ProductDTO>> GetAllProduct(Pagination pagination);
        public Task<ResponseObject<List<ProductDTO>>> GetRelatedProducts(int productID);
        public Task<ResponseObject<List<ProductDTO>>> GetFeaturedProduct();
        public Task<ResponseObject<ProductDTO>> GetProductByID(int productID);
        public Task<ResponseObject<ProductDTO>> GetProductByIDAndUpdateView(int productID);
        public Task<ResponseObject<ProductDTO>> CreateProduct(CreateProductRequest request);
        public Task<ResponseObject<ProductDTO>> UpdateProduct(int productID, UpdateProductRequest request);
        public Task<ResponseObject<string>> RemoveProduct(int productID);
    }
}
