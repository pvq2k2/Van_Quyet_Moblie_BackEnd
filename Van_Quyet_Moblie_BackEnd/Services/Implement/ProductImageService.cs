using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductImageService : BaseService, IProductImageService
    {
        private readonly ResponseObject<ProductImageDTO> _response;
        private readonly CloudinaryHelper _cloundinaryHelper;
        public ProductImageService(ResponseObject<ProductImageDTO> response, CloudinaryHelper cloudinaryHelper)
        {
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
                if (!await _context.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
                }
                InputHelper.IsImage(request.ImageProduct!);

                string image = await _cloundinaryHelper.UploadImage(request.ImageProduct!, $"van-quyet-mobile/productImage/{request.ProductID}", "product-image");
                var productImage = new ProductImage
                {
                    Title = request.Title,
                    ImageProduct = image,
                    ProductID = request.ProductID,
                    Status = (int)Status.Active
                };

                await _context.ProductImage.AddAsync(productImage);
                await _context.SaveChangesAsync();

                return _response.ResponseSuccess("Thêm ảnh sản phẩm thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<PageResult<ProductImageDTO>> GetAllProductImage(Pagination pagination)
        {
            var query = _context.ProductImage.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductImage>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductImageDTO>(pagination, _productImageConverter.ListEntityProductImageToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductImageDTO>> GetProductImageByID(int productImageID)
        {
            var productImage = await _context.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
            if (productImage == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Ảnh sản phẩm không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
        }

        public async Task<ResponseObject<string>> RemoveProductImage(int productImageID)
        {
            var response = new ResponseObject<string>();
            var productImage = await _context.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
            if (productImage == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Ảnh sản phẩm không tồn tại !", null!);
            }

            _context.ProductImage.Remove(productImage);
            await _context.SaveChangesAsync();

            return response.ResponseSuccess("Xóa ảnh sản phẩm thành công !", null!);
        }

        public async Task<ResponseObject<ProductImageDTO>> UpdateProductImage(int productImageID, UpdateProductImageRequest request)
        {
            try
            {
                var productImage = await _context.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
                if (productImage == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Ảnh sản phẩm không tồn tại !", null!);
                }

                if (string.IsNullOrEmpty(request.Title))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề !", null!);
                }

                if (!await _context.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
                }

                string img;
                if (request.ImageProduct == null || request.ImageProduct.Length == 0)
                {
                    img = productImage.ImageProduct!;
                }
                else
                {
                    InputHelper.IsImage(request.ImageProduct!);
                    img = await _cloundinaryHelper.UploadImage(request.ImageProduct!, $"van-quyet-mobile/productImage/{request.ProductID}", "product-image");
                    await _cloundinaryHelper.DeleteImageByUrl(productImage.ImageProduct!);
                }

                productImage.Title = request.Title;
                productImage.ImageProduct = img;
                productImage.ProductID = request.ProductID;
                productImage.Status = request.Status; 
                productImage.UpdatedAt = DateTime.Now;

                _context.ProductImage.Update(productImage);
                await _context.SaveChangesAsync();

                return _response.ResponseSuccess("Cập nhật ảnh sản phẩm thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
