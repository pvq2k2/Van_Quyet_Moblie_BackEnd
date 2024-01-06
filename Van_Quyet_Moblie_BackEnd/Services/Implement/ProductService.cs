using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductService : IProductService
    {
        private readonly ProductConverter _productConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<ProductDTO> _response;
        private readonly CloudinaryHelper _cloundinaryHelper;
        public ProductService(AppDbContext dbContext, ResponseObject<ProductDTO> response, CloudinaryHelper cloudinaryHelper)
        {
            _productConverter = new ProductConverter();
            _dbContext = dbContext;
            _response = response;
            _cloundinaryHelper = cloudinaryHelper;
        }
        public async Task<ResponseObject<ProductDTO>> CreateProduct(CreateProductRequest request)
        {
            try
            {
                InputHelper.CreateProductValidate(request);
                //if (!await _dbContext.ProductType.AnyAsync(x => x.ID == request.ProductTypeID))
                //{
                //    return _response.ResponseError(StatusCodes.Status404NotFound, "Danh mục không tồn tại !", null!);
                //}
                InputHelper.IsImage(request.Image!);

                string image = await _cloundinaryHelper.UploadImage(request.Image!, "van-quyet-mobile/product", "product");
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Image = image,
                    Discount = request.Discount,
                    Title = request.Title,
                    Status = (int)Status.Active,
                };

                await _dbContext.Product.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Thêm sản phẩm thành công !", _productConverter.EntityProductToDTO(product));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
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
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
            }
            return _response.ResponseSuccess("Thành công !", _productConverter.EntityProductToDTO(product));
        }

        public async Task<ResponseObject<ProductDTO>> GetProductByIDAndUpdateView(int productID)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
            if (product == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Sản phẩm không tồn tại !", null!);
            }
            product.NumberOfViews++;
            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Thành công !", _productConverter.EntityProductToDTO(product));
        }

        public async Task<ResponseObject<List<ProductDTO>>> GetRelatedProducts(int productID)
        {
            var response = new ResponseObject<List<ProductDTO>>();
            var currentProduct = await _dbContext.Product.FirstOrDefaultAsync(p => p.ID == productID);

            if (currentProduct == null)
            {
                return response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
            }

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
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
            if (product == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Sảm phẩm không tồn tại !", null!);
            }

            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Xóa sản phẩm thành công !", null!);
        }

        public async Task<ResponseObject<ProductDTO>> UpdateProduct(int productID, UpdateProductRequest request)
        {
            try
            {
                var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
                if (product == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }
                InputHelper.UpdateProductValidate(request);
                //if (!await _dbContext.ProductType.AnyAsync(x => x.ID == request.ProductTypeID))
                //{
                //    return _response.ResponseError(StatusCodes.Status404NotFound, "Danh mục không tồn tại !", null!);
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
                product.Title = request.Title;
                product.Image = img;
                product.Discount = request.Discount;
                product.Status = request.Status;
                product.UpdatedAt = DateTime.Now;

                _dbContext.Product.Update(product);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Cập nhật sản phẩm thành công !", _productConverter.EntityProductToDTO(product));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
