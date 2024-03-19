using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ColorRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class ColorService : IColorService
    {
        private readonly ColorConverter _colorConverter;
        private readonly AppDbContext _dbContext;
        private readonly TokenHelper _tokenHelper;
        private readonly Response _response;
        private readonly ResponseObject<ColorDTO> _responseColor;

        public ColorService(AppDbContext dbContext,
            TokenHelper tokenHelper,
            Response response,
            ResponseObject<ColorDTO> responseColor)
        {
            _colorConverter = new ColorConverter();
            _dbContext = dbContext;
            _tokenHelper = tokenHelper;
            _response = response;
            _responseColor = responseColor;
        }

        #region Validate
        private void IsAdmin()
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();
            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
        }
        private static void ValidateColor(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên màu không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giá trị màu không được để trống !");
            }
            if (!InputHelper.RegexColor(value))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giá trị màu phải là hex, rgb và hsl !");
            }
        }
        private async Task<Entities.Color> IsColorExist(int colorID)
        {
            var color = await _dbContext.Color.FirstOrDefaultAsync(x => x.ID == colorID);
            return color ?? throw new CustomException(StatusCodes.Status404NotFound, "Màu không tồn tại !");
        }
        #endregion
        public async Task<Response> CreateColor(CreateColorRequest request)
        {
            IsAdmin();
            ValidateColor(request.Name!, request.Value!);
            var color = new Entities.Color
            {
                Name = request.Name,
                Value = request.Value,
            };
            await _dbContext.Color.AddAsync(color);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Thêm màu thành công !");
        }

        public async Task<PageResult<ColorDTO>> GetAllColor(Pagination pagination)
        {
            IsAdmin();

            var query = _dbContext.Color.OrderByDescending(x => x.ID).AsQueryable();

            pagination.TotalCount = await query.CountAsync();
            var result = PageResult<Entities.Color>.ToPageResult(pagination, query);

            var list = result.ToList();

            return new PageResult<ColorDTO>(pagination, _colorConverter.ListEntityColorToDTO(result.ToList()));
        }

        public async Task<ResponseObject<ColorDTO>> GetColorByID(int colorID)
        {
            IsAdmin();
            var color = await IsColorExist(colorID);
            return _responseColor.ResponseSuccess("Thành công !", _colorConverter.EntityColorToDTO(color));
        }

        public async Task<Response> UpdateColor(int colorID, UpdateColorRequest request)
        {
            IsAdmin();
            ValidateColor(request.Name!, request.Value!);
            var color = await IsColorExist(colorID);

            color.Name = request.Name;
            color.Value = request.Value;
            color.UpdatedAt = DateTime.Now;

            _dbContext.Color.Update(color);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Cập nhật màu thành công !");
        }
    }
}
