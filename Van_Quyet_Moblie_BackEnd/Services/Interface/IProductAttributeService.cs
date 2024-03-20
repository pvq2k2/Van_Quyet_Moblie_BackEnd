using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductAttribute;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductAttributeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductAttributeService
    {
        public Task<PageResult<ProductAttributeDTO>> GetAllProductAttribute(Pagination pagination, int productID);
        public Task<ResponseObject<GetUpdateProductAttributeDTO>> GetUpdateProductAttributeByID(int productAttributeID);
        public Task<Response> CreateProductAttribute(CreateProductAttributeRequest request);
        public Task<Response> UpdateProductAttribute(int productAttributeID, UpdateProductAttributeRequest request);
        public Task<Response> RemoveProductAttribute(int productAttributeID);
    }
}
