using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductImage;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductImageService : IProductImageService
    {
        private readonly ProductImageConverter _productImageConverter;
        private readonly TokenHelper _tokenHelper;
        private readonly AppDbContext _dbContext;
        private readonly Response _response;
        private readonly ResponseObject<ProductImageDTO> _responseProductImage;
        private readonly ResponseObject<GetUpdateProductImageDTO> _responseGetUpdateProductImage;
        private readonly CloudinaryHelper _cloudinaryHelper;
        public ProductImageService(AppDbContext dbContext, ResponseObject<ProductImageDTO> responseProductImage, CloudinaryHelper cloudinaryHelper, TokenHelper tokenHelper, Response response, ResponseObject<GetUpdateProductImageDTO> responseGetUpdateProductImage)
        {
            _productImageConverter = new ProductImageConverter();
            _dbContext = dbContext;
            _responseProductImage = responseProductImage;
            _cloudinaryHelper = cloudinaryHelper;
            _tokenHelper = tokenHelper;
            _response = response;
            _responseGetUpdateProductImage = responseGetUpdateProductImage;
        }

        #region Validate
        private async Task IsProductExist(int productID)
        {
            if (!await _dbContext.Product.AnyAsync(x => x.ID == productID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !");
            }
        }
        private async Task<ProductImage> IsProductImageExist(int productImageID)
        {
            var productImage = await _dbContext.ProductImage.FirstOrDefaultAsync(x => x.ID == productImageID);
            return productImage ?? throw new CustomException(StatusCodes.Status404NotFound, "Ảnh sản phẩm không tồn tại !");
        }
        private static void ValidateProductImage(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên ảnh không được để trống !");
            }
        }
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
        }
        private async Task<string> UploadAndValidateImage(IFormFile image, string folderPath, string fileName)
        {
            InputHelper.IsImage(image);
            return await _cloudinaryHelper.UploadImage(image, folderPath, fileName);
        }
        private async Task IsColorExist(int color)
        {
            if (color != 0)
            {
                if (!await _dbContext.Color.AnyAsync(x => x.ID == color))
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Màu không tồn tại !");
                }
            }
        }
        #endregion

        public async Task<Response> CreateProductImage(CreateProductImageRequest request)
        {
            IsAdmin();
            ValidateProductImage(request.Title!);
            await IsProductExist(request.ProductID);
            await IsColorExist(request.ColorID);

            string image = await UploadAndValidateImage(request.Image!, $"van-quyet-mobile/productImage/{request.ProductID}", "product-image");

            var productImage = new ProductImage
            {
                Title = request.Title,
                Image = image,
                ProductID = request.ProductID,
                ColorID = request.ColorID,
                Status = (int)Status.Active
            };

            await _dbContext.ProductImage.AddAsync(productImage);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm ảnh sản phẩm thành công !");

        }

        public async Task<PageResult<ProductImageDTO>> GetAllProductImage(Pagination pagination, int productID)
        {
            await IsProductExist(productID);
            var query = _dbContext.ProductImage.Where(x => x.ProductID == productID).OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductImage>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductImageDTO>(pagination, _productImageConverter.ListEntityProductImageToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductImageDTO>> GetProductImageByID(int productImageID)
        {
            var productImage = await IsProductImageExist(productImageID);
            return _responseProductImage.ResponseSuccess("Thành công !", _productImageConverter.EntityProductImageToDTO(productImage));
        }

        public async Task<Response> RemoveProductImage(int productImageID)
        {
            IsAdmin();
            var productImage = await IsProductImageExist(productImageID);
            await _cloudinaryHelper.DeleteImageByUrl(productImage.Image!);
            _dbContext.ProductImage.Remove(productImage);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Xóa ảnh sản phẩm thành công !");
        }

        public async Task<Response> UpdateProductImage(int productImageID, UpdateProductImageRequest request)
        {
            IsAdmin();
            var productImage = await IsProductImageExist(productImageID);
            ValidateProductImage(request.Title!);
            await IsProductExist(request.ProductID);
            await IsColorExist(request.ColorID);
            string img;
            if (request.Image == null || request.Image.Length == 0)
            {
                img = productImage.Image!;
            }
            else
            {
                img = await UploadAndValidateImage(request.Image!, $"van-quyet-mobile/productImage/{request.ProductID}", "product-image");
                await _cloudinaryHelper.DeleteImageByUrl(productImage.Image!);
            }

            productImage.Title = request.Title;
            productImage.Image = img;
            productImage.ProductID = request.ProductID;
            productImage.Status = request.Status;
            productImage.UpdatedAt = DateTime.Now;

            _dbContext.ProductImage.Update(productImage);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật ảnh sản phẩm thành công !");
        }

        public async Task<ResponseObject<GetUpdateProductImageDTO>> GetUpdateProductImageByID(int productImageID)
        {
            var productImage = await IsProductImageExist(productImageID);
            return _responseGetUpdateProductImage.ResponseSuccess("Thành công !", _productImageConverter.EntityProductImageToGetUpdateProductImageDTO(productImage));
        }
    }
}
