using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductTypeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductTypeService : BaseService, IProductTypeService
    {
        private readonly ResponseObject<ProductTypeDTO> _response;
        private readonly CloudinaryHelper _cloundinaryHelper;
        public ProductTypeService(ResponseObject<ProductTypeDTO> response, CloudinaryHelper cloudinaryHelper)
        {
            _response = response;
            _cloundinaryHelper = cloudinaryHelper;
        }
        public async Task<ResponseObject<ProductTypeDTO>> CreateProductType(CreateProductTypeRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.NameProductType))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tên danh mục !", null!);
                }
                InputHelper.IsImage(request.ImageTypeProduct!);

                string image = await _cloundinaryHelper.UploadImage(request.ImageTypeProduct!, "van-quyet-mobile/productType", "product-type");
                var productType = new ProductType { 
                    NameProductType = request.NameProductType,
                    ImageTypeProduct = image
                };

                await _context.ProductType.AddAsync(productType);
                await _context.SaveChangesAsync();

                return _response.ResponseSuccess("Thêm danh mục thành công !", _productTypeConverter.EntityProductTypeToDTO(productType));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<PageResult<ProductTypeDTO>> GetAllProductType(Pagination pagination)
        {
            var query = _context.ProductType.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductType>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductTypeDTO>(pagination, _productTypeConverter.ListEntityProductTypeToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductTypeDTO>> GetProductTypeByID(int productTypeID)
        {
            var productType = await _context.ProductType.FirstOrDefaultAsync(x => x.ID == productTypeID);
            if (productType == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Danh mục không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _productTypeConverter.EntityProductTypeToDTO(productType));
        }

        public async Task<ResponseObject<string>> RemoveProductType(int productTypeID)
        {
            var response = new ResponseObject<string>();
            var productType = await _context.ProductType.FirstOrDefaultAsync(x => x.ID == productTypeID);
            if (productType == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Danh mục không tồn tại !", null!);
            }

            _context.ProductType.Remove(productType);
            await _context.SaveChangesAsync();

            return response.ResponseSuccess("Xóa danh mục thành công !", null!);
        }

        public async Task<ResponseObject<ProductTypeDTO>> UpdateProductType(int productTypeID, UpdateProductTypeRequest request)
        {
            try
            {
                var productType = await _context.ProductType.FirstOrDefaultAsync(x => x.ID == productTypeID);
                if (productType == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Danh mục không tồn tại !", null!);
                }

                if (string.IsNullOrEmpty(request.NameProductType))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Tên danh mục không được để trống !", null!);
                }
                string img;
                if (request.ImageTypeProduct == null || request.ImageTypeProduct.Length == 0)
                {
                    img = productType.ImageTypeProduct!;
                } else
                {
                    InputHelper.IsImage(request.ImageTypeProduct!);
                    img = await _cloundinaryHelper.UploadImage(request.ImageTypeProduct, "van-quyet-mobile/productType", "product-type");
                    await _cloundinaryHelper.DeleteImageByUrl(productType.ImageTypeProduct!);
                }

                productType.NameProductType = request.NameProductType;
                productType.ImageTypeProduct = img;
                productType.UpdatedAt = DateTime.Now;

                _context.ProductType.Update(productType);
                await _context.SaveChangesAsync();

                return _response.ResponseSuccess("Cập nhật danh mục thành công !", _productTypeConverter.EntityProductTypeToDTO(productType));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
