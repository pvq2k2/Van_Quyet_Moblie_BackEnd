using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly ProductReviewConverter _productReviewConverter;
        private readonly Response _response;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<ProductReviewDTO> _responseProductReview;
        private readonly TokenHelper _tokenHelper;
        public ProductReviewService(AppDbContext dbContext, ResponseObject<ProductReviewDTO> responseProductReview, TokenHelper tokenHelper, Response response)
        {
            _productReviewConverter = new ProductReviewConverter();
            _dbContext = dbContext;
            _responseProductReview = responseProductReview;
            _tokenHelper = tokenHelper;
            _response = response;
        }

        #region Private Function
        #region Validate
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status403Forbidden, "Không có quyền !");
            }
        }
        private async Task<int> IsUserExist()
        {
            _tokenHelper.IsToken();
            var userID = _tokenHelper.GetUserID();

            var isUserExist = await _dbContext.User.AnyAsync(x => x.ID == userID);

            if (isUserExist)
            {
                return userID;
            }
            throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại !");
        }
        private async Task<Product> IsProductExist(int productID)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == productID);
            return product ?? throw new CustomException(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !");
        }

        private async Task<ProductReview> IsProductReviewExist(int productReviewID)
        {
            var productReview = await _dbContext.ProductReview.FirstOrDefaultAsync(x => x.ID == productReviewID);
            return productReview ?? throw new CustomException(StatusCodes.Status404NotFound, "Bình luận không tồn tại !");
        }

        public void ProductReviewValidate(string contentRated, int pointEvaluation)
        {
            if (string.IsNullOrWhiteSpace(contentRated))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !");
            }

            //if (pointEvaluation < 1 || pointEvaluation > 5)
            //{
            //    throw new CustomException(StatusCodes.Status400BadRequest, "Số sao không được nhỏ hơn 1 hoặc lớn hơn 5 !");
            //}
        }
        #endregion
        #endregion
        public async Task<Response> CreateProductReview(CreateProductReviewRequest request)
        {
            ProductReviewValidate(request.ContentRated!, request.PointEvaluation);
            var product = await IsProductExist(request.ProductID);
            var userID = await IsUserExist();

            var productReview = new ProductReview
            {
                ProductID = request.ProductID,
                UserID = userID,
                ContentRated = request.ContentRated,
                PointEvaluation = request.PointEvaluation,
                ContentSeen = product.Name,
                Status = (int)Status.Active,
            };
            await _dbContext.ProductReview.AddAsync(productReview);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Bình luận thành công !");
        }

        public async Task<PageResult<ProductReviewDTO>> GetAllProductReview(Pagination pagination)
        {
            IsAdmin();
            var query = _dbContext.ProductReview.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductReview>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductReviewDTO>(pagination, _productReviewConverter.ListProductReviewToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductReviewDTO>> GetProductReviewByID(int productReviewID)
        {
            var productReview = await IsProductReviewExist(productReviewID);
            return _responseProductReview.ResponseSuccess("Thành công !", _productReviewConverter.EntityProductReviewToDTO(productReview));
        }

        public async Task<Response> RemoveProductReview(int productReviewID)
        {
            IsAdmin();
            var productReview = await IsProductReviewExist(productReviewID);

            _dbContext.ProductReview.Remove(productReview);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Xóa bình luận thành công !");
        }

        public async Task<Response> UpdateProductReview(int productReviewID, UpdateProductReviewRequest request)
        {
            IsAdmin();
            ProductReviewValidate(request.ContentRated!, request.PointEvaluation);
            var productReview = await IsProductReviewExist(productReviewID);
            var userID = await IsUserExist();
            var product = await IsProductExist(request.ProductID);

            productReview.ProductID = product.ID;
            productReview.UserID = userID;
            productReview.ContentRated = request.ContentRated;
            productReview.PointEvaluation = request.PointEvaluation;
            productReview.Status = request.Status;
            productReview.UpdatedAt = DateTime.Now;

            _dbContext.ProductReview.Update(productReview);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật bình luận thành công !");
        }
    }
}
