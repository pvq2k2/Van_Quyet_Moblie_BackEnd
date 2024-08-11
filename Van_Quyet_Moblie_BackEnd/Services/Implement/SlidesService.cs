using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Slides;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class SlidesService : ISlidesService
    {
        private readonly SlidesConverter _slidesConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<SlidesDTO> _responseSlides;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly Response _response;
        private readonly TokenHelper _tokenHelper;
        public SlidesService(AppDbContext dbContext, ResponseObject<SlidesDTO> responseSlides, CloudinaryHelper cloudinaryHelper, Response response, TokenHelper tokenHelper)
        {
            _slidesConverter = new SlidesConverter();
            _dbContext = dbContext;
            _responseSlides = responseSlides;
            _cloudinaryHelper = cloudinaryHelper;
            _response = response;
            _tokenHelper = tokenHelper;
        }
        #region Private Function
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status403Forbidden, "Không có quyền !");
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
        private async Task<Slides> IsSlidesExist(int slidesID)
        {
            var slide = await _dbContext.Slides.FirstOrDefaultAsync(x => x.ID == slidesID);
            return slide ?? throw new CustomException(StatusCodes.Status404NotFound, "Ảnh trình chiếu không tồn tại !");
        }
        private static void ValidateSlide(SlidesRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề !");
            }
            if (string.IsNullOrWhiteSpace(request.SubTitle))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề phụ !");
            }
        }
        #endregion
        public async Task<Response> CreateSlides(SlidesRequest request)
        {
            IsAdmin();
            ValidateSlide(request);
            var product = await IsProductExist(request.ProductID);
            string image = await UploadAndValidateImage(request.Image!, "van-quyet-mobile/slides", "slides");
            var slides = new Slides
            {
                Image = image,
                ProductID = request.ProductID,
                ProductSlug = product.Slug,
                Title = request.Title,
                SubTitle = request.SubTitle,
                Status = (int)Status.Active
            };

            await _dbContext.Slides.AddAsync(slides);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thêm ảnh trình chiếu thành công !");

        }

        public async Task<ResponseObject<List<GetActiveSlidesDTO>>> GetActiveSlides()
        {
            var response = new ResponseObject<List<GetActiveSlidesDTO>>();
            var listSlides = await _dbContext.Slides.Where(x => x.Status == (int)Status.Active).Take(10).OrderByDescending(x => x.ID).ToListAsync();

            return response.ResponseSuccess("Thành công !", _slidesConverter.ListEntitySlidesToGetActiveSlidesDTO(listSlides));
        }

        public async Task<PageResult<SlidesDTO>> GetAllSlides(Pagination pagination)
        {
            IsAdmin();
            var query = _dbContext.Slides.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Slides>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<SlidesDTO>(pagination, _slidesConverter.ListEntitySlidesToDTO(list));
        }

        public async Task<ResponseObject<SlidesDTO>> GetSlidesByID(int slidesID)
        {
            IsAdmin();
            var slides = await IsSlidesExist(slidesID);
            return _responseSlides.ResponseSuccess("Thành công !", _slidesConverter.EntitySlidesToDTO(slides));
        }

        public async Task<Response> RemoveSlides(int slidesID)
        {
            IsAdmin();
            var slides = await IsSlidesExist(slidesID);
            await _cloudinaryHelper.DeleteImageByUrl(slides.Image!);
            _dbContext.Slides.Remove(slides);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Xóa ảnh trình chiếu thành công !");
        }

        public async Task<Response> UpdateSlides(int slidesID, SlidesRequest request)
        {
            IsAdmin();
            ValidateSlide(request);

            var slides = await IsSlidesExist(slidesID);
            var product = await IsProductExist(request.ProductID);

            string img;
            if (request.Image == null || request.Image.Length == 0)
            {
                img = slides.Image!;
            }
            else
            {
                InputHelper.IsImage(request.Image!);
                img = await UploadAndValidateImage(request.Image, "van-quyet-mobile/slides", "slides");
                await _cloudinaryHelper.DeleteImageByUrl(slides.Image!);
            }

            slides.Image = img;
            slides.Status = request.Status;
            slides.ProductID = request.ProductID;
            slides.ProductSlug = product.Slug;
            slides.Title = request.Title;
            slides.SubTitle = request.SubTitle;
            slides.UpdatedAt = DateTime.Now;

            _dbContext.Slides.Update(slides);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Cập nhật ảnh trình chiếu thành công !");
        }
    }
}
