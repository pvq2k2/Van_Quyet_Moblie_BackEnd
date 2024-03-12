using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductAttributeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductService : IProductService
    {
        private readonly ProductConverter _productConverter;
        private readonly Response _response;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<ProductDTO> _responseProduct;
        private readonly CloudinaryHelper _cloundinaryHelper;
        public ProductService(AppDbContext dbContext, ResponseObject<ProductDTO> responseProduct, CloudinaryHelper cloudinaryHelper, Response response)
        {
            _productConverter = new ProductConverter();
            _dbContext = dbContext;
            _responseProduct = responseProduct;
            _cloundinaryHelper = cloudinaryHelper;
            _response = response;
        }

        #region Private Function
        #region Validate
        private async Task ValidateSubCategory(int subCategoryID)
        {
            if (!await _dbContext.SubCategories.AnyAsync(x => x.ID == subCategoryID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Danh mục không tồn tại !");
            }
        }
        private async Task<string> UploadAndValidateImage(IFormFile image, string folderPath, string fileName)
        {
            InputHelper.IsImage(image);
            return await _cloundinaryHelper.UploadImage(image, folderPath, fileName);
        }
        private async Task ValidateProductImage(CreateProductImageRequest item)
        {
            if (string.IsNullOrEmpty(item.Title))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên ảnh không được để trống !");
            }

            if (item.ColorID > 0)
            {
                await ValidateColor(item.ColorID);
            }
        }
        private async Task ValidateColor(int colorId)
        {
            if (!await _dbContext.Color.AnyAsync(x => x.ID == colorId))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Màu không tồn tại !");
            }
        }
        private async Task ValidateProductAttribute(CreateProductAttributeRequest item)
        {
            if (item.Quantity < 1)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Số lượng không được nhỏ hơn 1 !");
            }

            if (item.Price < 1)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giá không được nhỏ hơn 1 !");
            }

            await ValidateColor(item.ColorID);
            await ValidateSize(item.SizeID);
        }
        private async Task ValidateSize(int sizeId)
        {
            if (!await _dbContext.Size.AnyAsync(x => x.ID == sizeId))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Kích cỡ không tồn tại !");
            }
        }
        private async Task<Product> IsProductExist(int productID)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
            return product ?? throw new CustomException(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !");
        }
        #endregion
        private static Product CreateProductObject(CreateProductRequest request, string image)
        {
            return new Product
            {
                Name = request.Name,
                Price = request.Price,
                Image = image,
                Discount = request.Discount,
                Description = request.Description,
                Height = request.Height,
                Width = request.Width,
                Length = request.Length,
                Weight = request.Weight,
                SubCategoriesID = request.SubCategoriesID,
                Status = (int)Status.Active,
            };
        }
        private async Task ProcessProductImages(List<CreateProductImageRequest> productImages, int productId, List<string> listImageTemp)
        {
            if (productImages != null && productImages.Any())
            {
                foreach (var item in productImages)
                {
                    await ValidateProductImage(item);

                    string img = await _cloundinaryHelper.UploadImage(item.Image!, "van-quyet-mobile/product-image", "product-image");
                    listImageTemp.Add(img);

                    var productImage = new ProductImage
                    {
                        Title = item.Title,
                        Image = img,
                        ProductID = productId,
                        ColorID = item.ColorID,
                        Status = (int)Status.Active
                    };

                    await _dbContext.ProductImage.AddAsync(productImage);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        private async Task ProcessProductAttributes(List<CreateProductAttributeRequest> productAttributes, int productId)
        {
            if (productAttributes != null && productAttributes.Any())
            {
                var listProductAttribute = new List<ProductAttribute>();

                foreach (var item in productAttributes)
                {
                    await ValidateProductAttribute(item);

                    var productAttribute = new ProductAttribute
                    {
                        ProductID = productId,
                        SizeID = item.SizeID,
                        ColorID = item.ColorID,
                        Quantity = item.Quantity,
                        Price = item.Price,
                    };

                    listProductAttribute.Add(productAttribute);
                }

                await _dbContext.ProductAttribute.AddRangeAsync(listProductAttribute);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion
        public async Task<Response> CreateProduct(CreateProductRequest request)
        {
            var listImageTemp = new List<string>();
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                InputHelper.CreateProductValidate(request);
                await ValidateSubCategory(request.SubCategoriesID);
                string image = await UploadAndValidateImage(request.Image!, "van-quyet-mobile/product", "product");
                listImageTemp.Add(image);
                var product = CreateProductObject(request, image);
                await _dbContext.Product.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                await ProcessProductImages(request.ListProductImage!, product.ID, listImageTemp);
                await ProcessProductAttributes(request.ListProductAttribute!, product.ID);

                await transaction.CommitAsync();
                return _response.ResponseSuccess("Tạo sản phẩm thành công !");
            }
            catch (Exception ex)
            {
                if (listImageTemp.Count > 0)
                {
                    foreach (var item in listImageTemp)
                    {
                        await _cloundinaryHelper.DeleteImageByUrl(item);
                    }
                }
                await transaction.RollbackAsync();
                throw new CustomException(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<PageResult<ProductDTO>> GetAllProduct(Pagination pagination)
        {
            var query = _dbContext.Product.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Product>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductDTO>(pagination, _productConverter.ListEntityProductToDTO(result.ToList()));
        }

        public async Task<ResponseObject<List<ProductDTO>>> GetFeaturedProduct()
        {
            var response = new ResponseObject<List<ProductDTO>>();

            var topProducts = await _dbContext.Product
            .OrderByDescending(p => p.NumberOfViews)
            .Take(10)
            .ToListAsync();

            return response.ResponseSuccess("Thành công !", _productConverter.ListEntityProductToDTO(topProducts));
        }

        public async Task<ResponseObject<ProductDTO>> GetProductByID(int productID)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
            if (product == null)
            {
                return _responseProduct.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
            }
            return _responseProduct.ResponseSuccess("Thành công !", _productConverter.EntityProductToDTO(product));
        }

        public async Task<ResponseObject<ProductDTO>> GetProductByIDAndUpdateView(int productID)
        {
            var product = await IsProductExist(productID);
            product.NumberOfViews++;
            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();
            return _responseProduct.ResponseSuccess("Thành công !", _productConverter.EntityProductToDTO(product));
        }

        public async Task<ResponseObject<List<ProductDTO>>> GetRelatedProducts(int productID)
        {
            var response = new ResponseObject<List<ProductDTO>>();
            var currentProduct = await IsProductExist(productID);

            var relatedProducts = _dbContext.Product
            .Where(p => p.SubCategoriesID == currentProduct.SubCategoriesID && p.ID != currentProduct.ID)
            .OrderByDescending(p => p.NumberOfViews)
            .Take(5)
            .ToList();

            return response.ResponseSuccess("Thành công !", _productConverter.ListEntityProductToDTO(relatedProducts));

        }

        public async Task<ResponseObject<string>> RemoveProduct(int productID)
        {
            var response = new ResponseObject<string>();
            var product = await IsProductExist(productID);

            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Xóa sản phẩm thành công !", null!);
        }

        public async Task<ResponseObject<ProductDTO>> UpdateProduct(int productID, UpdateProductRequest request)
        {
            try
            {
                var product = await IsProductExist(productID);
                InputHelper.UpdateProductValidate(request);
                //if (!await _dbContext.ProductType.AnyAsync(x => x.ID == request.ProductTypeID))
                //{
                //    return _responseProduct.ResponseError(StatusCodes.Status404NotFound, "Danh mục không tồn tại !", null!);
                //}

                string img;
                if (request.Image == null || request.Image.Length == 0)
                {
                    img = product.Image!;
                }
                else
                {
                    InputHelper.IsImage(request.Image!);
                    img = await _cloundinaryHelper.UploadImage(request.Image, "van-quyet-mobile/product", "product");
                    await _cloundinaryHelper.DeleteImageByUrl(product.Image!);
                }

                product.Name = request.Name;
                product.Price = request.Price;
                product.Description = request.Description;
                product.Image = img;
                product.Discount = request.Discount;
                product.Status = request.Status;
                product.UpdatedAt = DateTime.Now;

                _dbContext.Product.Update(product);
                await _dbContext.SaveChangesAsync();

                return _responseProduct.ResponseSuccess("Cập nhật sản phẩm thành công !", _productConverter.EntityProductToDTO(product));
            }
            catch (Exception e)
            {
                return _responseProduct.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
