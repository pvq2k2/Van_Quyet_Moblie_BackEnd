using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class SlidesService : ISlidesService
    {
        private readonly SlidesConverter _slidesConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<SlidesDTO> _response;
        private readonly CloudinaryHelper _cloundinaryHelper;
        public SlidesService(AppDbContext dbContext, ResponseObject<SlidesDTO> response, CloudinaryHelper cloudinaryHelper)
        {
            _slidesConverter = new SlidesConverter();
            _dbContext = dbContext;
            _response = response;
            _cloundinaryHelper = cloudinaryHelper;
        }
        public async Task<ResponseObject<SlidesDTO>> CreateSlides(CreateSlidesRequest request)
        {
            try
            {
                InputHelper.IsImage(request.Image!);
                if (!await _dbContext.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.SubTitle))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề phụ !", null!);
                }
                string image = await _cloundinaryHelper.UploadImage(request.Image!, "van-quyet-mobile/slides", "slides");
                var slides = new Slides
                {
                    Image = image,
                    ProductID = request.ProductID,
                    Title = request.Title,
                    SubTitle = request.SubTitle,
                    Status = (int)Status.Active
                };

                await _dbContext.Slides.AddAsync(slides);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Thêm slides thành công !", _slidesConverter.EntitySlidesToDTO(slides));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<ResponseObject<List<SlidesDTO>>> GetActiveSlides()
        {
            var response = new ResponseObject<List<SlidesDTO>>();
            var listSlides = await _dbContext.Slides.Where(x => x.Status == (int)Status.Active).ToListAsync();

            return response.ResponseSuccess("Thành công !", _slidesConverter.ListEntitySlidesToDTO(listSlides));
        }

        public async Task<PageResult<SlidesDTO>> GetAllSlides(Pagination pagination)
        {
            var query = _dbContext.Slides.OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Slides>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<SlidesDTO>(pagination, _slidesConverter.ListEntitySlidesToDTO(list));
        }

        public async Task<ResponseObject<SlidesDTO>> GetSlidesByID(int slidesID)
        {
            var slides = await _dbContext.Slides.FirstOrDefaultAsync(x => x.ID == slidesID);
            if (slides == null)
            {
                return _response.ResponseError(StatusCodes.Status404NotFound, "Slides không tồn tại !", null!);
            }
            return _response.ResponseSuccess("Thành công !", _slidesConverter.EntitySlidesToDTO(slides));
        }

        public async Task<ResponseObject<string>> RemoveSlides(int slidesID)
        {
            var response = new ResponseObject<string>();
            var slides = await _dbContext.Slides.FirstOrDefaultAsync(x => x.ID == slidesID);
            if (slides == null)
            {
                return response.ResponseError(StatusCodes.Status404NotFound, "Slides không tồn tại !", null!);
            }
            _dbContext.Slides.Remove(slides);
            await _dbContext.SaveChangesAsync();
            return response.ResponseSuccess("Xóa slides thành công !", null!);
        }

        public async Task<ResponseObject<SlidesDTO>> UpdateSlides(int slidesID, UpdateSlidesRequest request)
        {
            try
            {
                var slides = await _dbContext.Slides.FirstOrDefaultAsync(x => x.ID == slidesID);
                if (slides == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Slides không tồn tại !", null!);
                }
                if (!await _dbContext.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề !", null!);
                }
                if (string.IsNullOrWhiteSpace(request.SubTitle))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập tiêu đề phụ !", null!);
                }

                string img;
                if (request.Image == null || request.Image.Length == 0)
                {
                    img = slides.Image!;
                }
                else
                {
                    InputHelper.IsImage(request.Image!);
                    img = await _cloundinaryHelper.UploadImage(request.Image, "van-quyet-mobile/slides", "slides");
                    await _cloundinaryHelper.DeleteImageByUrl(slides.Image!);
                }

                slides.Image = img;
                slides.Status = request.Status;
                slides.ProductID = request.ProductID;
                slides.Title = request.Title;
                slides.SubTitle = request.SubTitle;
                slides.UpdatedAt = DateTime.Now;

                _dbContext.Slides.Update(slides);
                await _dbContext.SaveChangesAsync();

                return _response.ResponseSuccess("Slides sản phẩm thành công !", _slidesConverter.EntitySlidesToDTO(slides));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
