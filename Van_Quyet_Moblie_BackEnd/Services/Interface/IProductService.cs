using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductService
    {
        public Task<PageResult<ProductDTO>> GetAllProduct(Pagination pagination);
        public Task<PageResult<ProductDTO>> GetAllProductByCategory(GetAllProductRequest request);
        public Task<ResponseObject<List<ProductDTO>>> GetRelatedProducts(int productID);
        public Task<ResponseObject<List<ProductDTO>>> GetFeaturedProduct();
        public Task<ResponseObject<ProductDTO>> GetProductByID(int productID);
        public Task<ResponseObject<GetUpdateProductDTO>> GetUpdateProductByID(int productID);
        public Task<ResponseObject<ProductDTO>> GetProductByIDAndUpdateView(int productID);
        public Task<Response> CreateProduct(CreateProductRequest request);
        public Task<object> GetProductBySlug(string request);
        public Task<Response> UpdateProduct(int productID, UpdateProductRequest request);
        public Task<ResponseObject<string>> RemoveProduct(int productID);
    }
}
