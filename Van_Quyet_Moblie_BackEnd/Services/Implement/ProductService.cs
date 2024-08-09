using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products;
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
        private readonly TokenHelper _tokenHelper;
        private readonly ResponseObject<ProductDTO> _responseProduct;
        private readonly ResponseObject<GetUpdateProductDTO> _responseGetUpdateProduct;
        private readonly CloudinaryHelper _cloudinaryHelper;
        public ProductService(AppDbContext dbContext, ResponseObject<ProductDTO> responseProduct, CloudinaryHelper cloudinaryHelper, Response response, ResponseObject<GetUpdateProductDTO> responseGetUpdateProduct, TokenHelper tokenHelper)
        {
            _productConverter = new ProductConverter();
            _dbContext = dbContext;
            _responseProduct = responseProduct;
            _cloudinaryHelper = cloudinaryHelper;
            _response = response;
            _responseGetUpdateProduct = responseGetUpdateProduct;
            _tokenHelper = tokenHelper;
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
        private async Task<Product> IsProductExist(int productID)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
            return product ?? throw new CustomException(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !");
        }
        #endregion
        private static Product CreateProductObject(CreateProductRequest request, string image)
        {
            string slug = InputHelper.CreateSlug(request.Name!);
            return new Product
            {
                Name = request.Name,
                Price = request.Price,
                Slug = slug,
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
        #endregion
        public async Task<Response> CreateProduct(CreateProductRequest request)
        {
            IsAdmin();
            InputHelper.CreateProductValidate(request);
            await ValidateSubCategory(request.SubCategoriesID);
            string image = await UploadAndValidateImage(request.Image!, "van-quyet-mobile/product", "product");
            var product = CreateProductObject(request, image);
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Tạo sản phẩm thành công !");
        }

        public async Task<PageResult<ProductDTO>> GetAllProduct(Pagination pagination)
        {
            var query = _dbContext.Product.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Product>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductDTO>(pagination, _productConverter.ListEntityProductToDTO(result.ToList()));
        }

        public async Task<PageResult<ProductDTO>> GetAllProductByCategory(GetAllProductRequest request)
        {
            var query = _dbContext.Product.Include(p => p.SubCategories)
                        .ThenInclude(sc => sc!.Categories)
                        .Where(p => p.SubCategories!.CategoriesID == request.CategoryID);

            if (request.SubCategoryID > 0)
            {
                query = query.Where(p => p.SubCategoriesID == request.SubCategoryID);
            }

            if (request.Price > 0)
            {
                var priceConditions = new Dictionary<int, Expression<Func<Product, bool>>>
        {
            { 1, p => p.Price < 1000000 },
            { 2, p => p.Price >= 1000000 && p.Price <= 2000000 },
            { 3, p => p.Price >= 2000000 && p.Price <= 3000000 },
            { 4, p => p.Price >= 3000000 && p.Price <= 4000000 },
            { 5, p => p.Price >= 4000000 && p.Price <= 5000000 },
            { 6, p => p.Price >= 5000000 && p.Price <= 6000000 },
            { 7, p => p.Price >= 6000000 && p.Price <= 8000000 },
            { 8, p => p.Price >= 8000000 && p.Price <= 10000000 },
            { 9, p => p.Price >= 10000000 && p.Price <= 12000000 },
            { 10, p => p.Price >= 12000000 && p.Price <= 15000000 },
            { 11, p => p.Price >= 15000000 && p.Price <= 20000000 },
            { 12, p => p.Price >= 20000000 },
        };

                if (priceConditions.TryGetValue(request.Price, out var priceCondition))
                {
                    query = query.Where(priceCondition);
                }
            }

            if (request.Storage > 0)
            {
                query = query.Where(p => p.ListProductAttribute!.Any(pa => pa.SizeID == request.Storage));
            }

            if (request.Option > 0)
            {
                query = request.Option switch
                {
                    1 => query.OrderByDescending(x => x.CreatedAt),
                    2 => query.OrderBy(x => x.Price),
                    3 => query.OrderByDescending(x => x.Price),
                    4 => query.OrderByDescending(x => x.NumberOfViews),
                    _ => query
                };
            }
            else
            {
                query = query.OrderByDescending(x => x.ID);
            }

            var pagination = new Pagination() { PageSize = request.PageSize, PageNumber = request.PageNumber };
            pagination.TotalCount = await query.CountAsync();

            var paginatedQuery = query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                      .Take(pagination.PageSize);

            var products = await paginatedQuery.ToListAsync();

            var productDTOs = _productConverter.ListEntityProductToDTO(products);

            return new PageResult<ProductDTO>(pagination, productDTOs);
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
            var product = await IsProductExist(productID);
            return _responseProduct.ResponseSuccess("Thành công !", _productConverter.EntityProductToDTO(product));
        }
        public async Task<ResponseObject<GetUpdateProductDTO>> GetUpdateProductByID(int productID)
        {
            IsAdmin();
            var product = await IsProductExist(productID);
            return _responseGetUpdateProduct.ResponseSuccess("Thành công !", _productConverter.EntityProductToGetUpdateProductDTO(product));
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

        public async Task<Response> UpdateProduct(int productID, UpdateProductRequest request)
        {
            IsAdmin();
            var product = await IsProductExist(productID);
            InputHelper.UpdateProductValidate(request);
            await ValidateSubCategory(request.SubCategoriesID);

            string img;
            if (request.Image == null || request.Image.Length == 0)
            {
                img = product.Image!;
            }
            else
            {
                img = await UploadAndValidateImage(request.Image!, "van-quyet-mobile/product", "product");
                await _cloudinaryHelper.DeleteImageByUrl(product.Image!);
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description;
            product.Image = img;
            product.Discount = request.Discount;
            product.Status = request.Status;
            product.SubCategoriesID = request.SubCategoriesID;
            product.Height = request.Height;
            product.Width = request.Width;
            product.Length = request.Length;
            product.Weight = request.Weight;
            product.Slug = InputHelper.CreateSlug(request.Name!);
            product.UpdatedAt = DateTime.Now;

            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật sản phẩm thành công !");
        }
    }
}
