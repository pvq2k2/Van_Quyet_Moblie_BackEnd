using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductTypeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly ProductTypeConverter _productTypeConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<ProductTypeDTO> _response;
        public ProductTypeService(AppDbContext dbContext, ResponseObject<ProductTypeDTO> response)
        {
            _productTypeConverter = new ProductTypeConverter();
            _dbContext = dbContext;
            _response = response;
        }
        public async Task<ResponseObject<ProductTypeDTO>> CreateProductType(CreateProductTypeRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Color))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập màu sắc !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.Size))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập kích cỡ !", null!);
                }
                if (request.Quantity < 0)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập số lượng lớn hơn 0 !", null!);
                }
                var productType = new ProductType {
                    Color = request.Color,
                    Size = request.Size,
                    Quantity = request.Quantity
                };

                await _dbContext.ProductType.AddAsync(productType);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Thêm loại sản phẩm thành công !", _productTypeConverter.EntityProductTypeToDTO(productType));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<PageResult<ProductTypeDTO>> GetAllProductType(Pagination pagination)
        {
            var query = _dbContext.ProductType.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductType>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductTypeDTO>(pagination, _productTypeConverter.ListEntityProductTypeToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductTypeDTO>> GetProductTypeByID(int productTypeID)
        {
            var productType = await _dbContext.ProductType.FirstOrDefaultAsync(x => x.ID == productTypeID);
            if (productType == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Loại sản phẩm không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _productTypeConverter.EntityProductTypeToDTO(productType));
        }

        public async Task<ResponseObject<string>> RemoveProductType(int productTypeID)
        {
            var response = new ResponseObject<string>();
            var productType = await _dbContext.ProductType.FirstOrDefaultAsync(x => x.ID == productTypeID);
            if (productType == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Loại sản phẩm không tồn tại !", null!);
            }

            _dbContext.ProductType.Remove(productType);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Xóa loại sản phẩm thành công !", null!);
        }

        public async Task<ResponseObject<ProductTypeDTO>> UpdateProductType(int productTypeID, UpdateProductTypeRequest request)
        {
            try
            {
                var productType = await _dbContext.ProductType.FirstOrDefaultAsync(x => x.ID == productTypeID);
                if (productType == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Loại sản phẩm không tồn tại !", null!);
                }

                if (string.IsNullOrWhiteSpace(request.Color))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập màu sắc !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.Size))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập kích cỡ !", null!);
                }
                if (request.Quantity < 0)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập số lượng lớn hơn 0 !", null!);
                }

                productType.Color = request.Color;
                productType.Size = request.Size;
                productType.Quantity = request.Quantity;
                productType.UpdatedAt = DateTime.Now;

                _dbContext.ProductType.Update(productType);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Cập nhật loại sản phẩm thành công !", _productTypeConverter.EntityProductTypeToDTO(productType));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
