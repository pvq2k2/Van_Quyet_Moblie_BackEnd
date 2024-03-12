using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductImageService : IProductImageService
    {
        private readonly ProductImageConverter _productImageConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<ProductImageDTO> _response;
        private readonly CloudinaryHelper _cloundinaryHelper;
        public ProductImageService(AppDbContext dbContext, ResponseObject<ProductImageDTO> response, CloudinaryHelper cloudinaryHelper)
        {
            _productImageConverter = new ProductImageConverter();
            _dbContext = dbContext;
            _response = response;
            _cloundinaryHelper = cloudinaryHelper;
        }

        public async Task<ResponseObject<ProductImageDTO>> CreateProductImage(CreateProductImageRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề !", null!);
                }
                if (!await _dbContext.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
                }
                InputHelper.IsImage(request.Image!);

                string image = await _cloundinaryHelper.UploadImage(request.Image!, $"van-quyet-mobile/productImage/{request.ProductID}", "product-image");
                var productImage = new ProductImage
                {
                    Title = request.Title,
                    Image = image,
                    ProductID = request.ProductID,
                    Status = (int)Status.Active
                };

                await _dbContext.ProductImage.AddAsync(productImage);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Thêm ảnh sản phẩm thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<PageResult<ProductImageDTO>> GetAllProductImage(Pagination pagination)
        {
            var query = _dbContext.ProductImage.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductImage>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductImageDTO>(pagination, _productImageConverter.ListEntityProductImageToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductImageDTO>> GetProductImageByID(int productImageID)
        {
            var productImage = await _dbContext.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
            if (productImage == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Ảnh sản phẩm không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
        }

        public async Task<ResponseObject<string>> RemoveProductImage(int productImageID)
        {
            var response = new ResponseObject<string>();
            var productImage = await _dbContext.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
            if (productImage == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Ảnh sản phẩm không tồn tại !", null!);
            }

            _dbContext.ProductImage.Remove(productImage);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Xóa ảnh sản phẩm thành công !", null!);
        }

        public async Task<ResponseObject<ProductImageDTO>> UpdateProductImage(int productImageID, UpdateProductImageRequest request)
        {
            try
            {
                var productImage = await _dbContext.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
                if (productImage == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Ảnh sản phẩm không tồn tại !", null!);
                }

                if (string.IsNullOrEmpty(request.Title))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề !", null!);
                }

                if (!await _dbContext.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
                }

                string img;
                if (request.Image == null || request.Image.Length == 0)
                {
                    img = productImage.Image!;
                }
                else
                {
                    InputHelper.IsImage(request.Image!);
                    img = await _cloundinaryHelper.UploadImage(request.Image!, $"van-quyet-mobile/productImage/{request.ProductID}", "product-image");
                    await _cloundinaryHelper.DeleteImageByUrl(productImage.Image!);
                }

                productImage.Title = request.Title;
                productImage.Image = img;
                productImage.ProductID = request.ProductID;
                productImage.Status = request.Status; 
                productImage.UpdatedAt = DateTime.Now;

                _dbContext.ProductImage.Update(productImage);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Cập nhật ảnh sản phẩm thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
