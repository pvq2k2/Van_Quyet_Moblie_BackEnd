using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly ProductReviewConverter _productReviewConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<ProductReviewDTO> _response;
        private readonly TokenHelper _tokenHelper;
        public ProductReviewService(AppDbContext dbContext, ResponseObject<ProductReviewDTO> response, TokenHelper tokenHelper)
        {
            _productReviewConverter = new ProductReviewConverter();
            _dbContext = dbContext;
            _response = response;
            _tokenHelper = tokenHelper;
        }
        public async Task<ResponseObject<ProductReviewDTO>> CreateProductReview(CreateProductReviewRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContentRated))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !", null!);
                }

                if (request.PointEvaluation < 1 || request.PointEvaluation > 5)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Số sao không được nhỏ hơn 1 hoặc lớn hơn 5 !", null!);
                }
                var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == request.ProductID);
                if (product == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();

                if (!await _dbContext.User.AnyAsync(x => x.ID == userID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại !", null!);
                }

                var productReview = new ProductReview
                {
                    ProductID = request.ProductID,
                    UserID = userID,
                    ContentRated = request.ContentRated,
                    PointEvaluation = request.PointEvaluation,
                    ContentSeen = product.Name,
                    Status = (int)Status.Active,
                };

                return _response.ResponseSuccess("Thêm bình luận thành công !", _productReviewConverter.EntityProductReviewToDTO(productReview));

            }
            catch (Exception ex)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }

        public async Task<PageResult<ProductReviewDTO>> GetAllProductReview(Pagination pagination)
        {
            var query = _dbContext.ProductReview.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<ProductReview>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<ProductReviewDTO>(pagination, _productReviewConverter.ListProductReviewToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ProductReviewDTO>> GetProductReviewByID(int productReviewID)
        {
            var productReview = await _dbContext.ProductReview.FirstOrDefaultAsync(x => x.ID == productReviewID);
            if (productReview == null)
            {
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Bình luận không tồn tại không tồn tại !", null!);
            }

            return _response.ResponseSuccess("Thành công !", _productReviewConverter.EntityProductReviewToDTO(productReview));
        }

        public async Task<ResponseObject<string>> RemoveProductReview(int productReviewID)
        {
            var response = new ResponseObject<string>();
            var productReview = await _dbContext.ProductReview.FirstOrDefaultAsync(x => x.ID == productReviewID);
            if (productReview == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Bình luận không tồn tại !", null!);
            }

            _dbContext.ProductReview.Remove(productReview);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Xóa bình luận thành công !", null!);
        }

        public async Task<ResponseObject<ProductReviewDTO>> UpdateProductReview(int productReviewID, UpdateProductReviewRequest request)
        {
            try
            {
                var productReview = await _dbContext.ProductReview.FirstOrDefaultAsync(x => x.ID == productReviewID);

                if (productReview == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Bình luận không tồn tại !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.ContentRated))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !", null!);
                }

                if (request.PointEvaluation < 1 || request.PointEvaluation > 5)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Số sao không được nhỏ hơn 1 hoặc lớn hơn 5 !", null!);
                }
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();

                if (!await _dbContext.User.AnyAsync(x => x.ID == userID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại !", null!);
                }

                var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.ID == request.ProductID);
                if (product == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }

                productReview.ProductID = request.ProductID;
                productReview.UserID = userID;
                productReview.ContentRated = request.ContentRated;
                productReview.PointEvaluation = request.PointEvaluation;
                productReview.Status = request.Status;
                productReview.UpdatedAt = DateTime.Now;

                return _response.ResponseSuccess("Cập nhật bình luận thành công !", _productReviewConverter.EntityProductReviewToDTO(productReview));
            }
            catch (Exception ex)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }
    }
}
