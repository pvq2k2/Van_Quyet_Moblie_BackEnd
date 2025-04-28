using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductAttribute;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductAttributeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly ProductAttributeConverter _productAttributeConverter;
        private readonly TokenHelper _tokenHelper;
        private readonly AppDbContext _dbContext;
        private readonly Response _response;
        private readonly ResponseObject<GetUpdateProductAttributeDTO> _responseGetUpdateProductAttribute;

        public ProductAttributeService(AppDbContext dbContext, TokenHelper tokenHelper, Response response, ResponseObject<GetUpdateProductAttributeDTO> responseGetUpdateProductAttribute)
        {
            _productAttributeConverter = new ProductAttributeConverter();
            _tokenHelper = tokenHelper;
            _dbContext = dbContext;
            _response = response;
            _responseGetUpdateProductAttribute = responseGetUpdateProductAttribute;
        }

        #region Validate
        private async Task IsProductExist(int productID)
        {
            if (!await _dbContext.Product.AnyAsync(x => x.ID == productID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !");
            }
        }
        private async Task IsColorExist(int colorID)
        {
            if (!await _dbContext.Color.AnyAsync(x => x.ID == colorID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Màu không tồn tại !");
            }
        }
        private async Task IsSizeExist(int sizeID)
        {
            if (!await _dbContext.Size.AnyAsync(x => x.ID == sizeID))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Kích cỡ không tồn tại !");
            }
        }
        private async Task<ProductAttribute> IsProductAttributeExist(int productAttributeID)
        {
            var productAttribute = await _dbContext.ProductAttribute.FirstOrDefaultAsync(x => x.ID == productAttributeID);
            return productAttribute ?? throw new CustomException(StatusCodes.Status404NotFound, "Thuộc tính sản phẩm không tồn tại !");
        }
        private static void ValidateProductAttribute(int quantity, double price)
        {
            if (quantity < 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Số lượng không được nhỏ hơn 0 !");
            }
            if (price < 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giá tiền không được nhỏ hơn 0 !");
            }
        }
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status403Forbidden, "Không có quyền !");
            }
        }
        #endregion
        public async Task<Response> CreateProductAttribute(CreateProductAttributeRequest request)
        {
            IsAdmin();
            ValidateProductAttribute(request.Quantity, request.Price);
            await IsProductExist(request.ProductID);
            await IsColorExist(request.ColorID);
            await IsSizeExist(request.SizeID);

            var productAttribute = new ProductAttribute
            {
                ProductID = request.ProductID,
                ColorID = request.ColorID,
                SizeID = request.SizeID,
                Quantity = request.Quantity,
                Price = request.Price,
            };

            await _dbContext.ProductAttribute.AddAsync(productAttribute);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm thuộc tính sản phẩm thành công !");
        }

        public async Task<PageResult<ProductAttributeDTO>> GetAllProductAttribute(Pagination pagination, int productID)
        {
            IsAdmin();
            await IsProductExist(productID);
            var query = _dbContext.ProductAttribute.Where(x => x.ProductID == productID).Include(x => x.Color).Include(x => x.Size).OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductAttribute>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductAttributeDTO>(pagination, _productAttributeConverter.ListEntityProductAttributeToDTO(result.ToList()));
        }

        public async Task<ResponseObject<GetUpdateProductAttributeDTO>> GetUpdateProductAttributeByID(int productAttributeID)
        {
            IsAdmin();
            var productAttribute = await IsProductAttributeExist(productAttributeID);
            return _responseGetUpdateProductAttribute.ResponseSuccess("Thành công !", _productAttributeConverter.EntityProductAttributeToGetUpdateProductAttributeDTO(productAttribute));
        }

        public async Task<Response> RemoveProductAttribute(int productAttributeID)
        {
            IsAdmin();
            var productAttribute = await IsProductAttributeExist(productAttributeID);
            _dbContext.ProductAttribute.Remove(productAttribute);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Xóa thuộc tính sản phẩm thành công !");
        }

        public async Task<Response> UpdateProductAttribute(int productAttributeID, UpdateProductAttributeRequest request)
        {
            IsAdmin();
            ValidateProductAttribute(request.Quantity, request.Price);
            var productAttribute = await IsProductAttributeExist(productAttributeID);
            await IsProductExist(request.ProductID);
            await IsColorExist(request.ColorID);
            await IsSizeExist(request.SizeID);

            productAttribute.ProductID = request.ProductID;
            productAttribute.ColorID = request.ColorID;
            productAttribute.SizeID = request.SizeID;
            productAttribute.Price = request.Price;
            productAttribute.Quantity = request.Quantity;
            productAttribute.UpdatedAt = DateTime.Now;

            _dbContext.ProductAttribute.Update(productAttribute);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Cập nhật thuộc tính sản phẩm thành công !");
        }
    }
}
