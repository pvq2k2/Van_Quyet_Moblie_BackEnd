using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductTypeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IProductTypeService
    {
        public Task<PageResult<ProductTypeDTO>> GetAllProductType(Pagination pagination);
        public Task<ResponseObject<ProductTypeDTO>> GetProductTypeByID(int productTypeID);
        public Task<ResponseObject<ProductTypeDTO>> CreateProductType(CreateProductTypeRequest request);
        public Task<ResponseObject<ProductTypeDTO>> UpdateProductType(int productTypeID, UpdateProductTypeRequest request);
        public Task<ResponseObject<string>> RemoveProductType(int productTypeID);
    }
}
