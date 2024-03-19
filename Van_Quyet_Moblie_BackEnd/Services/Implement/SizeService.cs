using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SizeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class SizeService : ISizeService
    {
        private readonly SizeConverter _sizeConverter;
        private readonly AppDbContext _dbContext;
        private readonly TokenHelper _tokenHelper;
        private readonly Response _response;
        private readonly ResponseObject<SizeDTO> _responseSize;

        public SizeService(AppDbContext dbContext,
            TokenHelper tokenHelper,
            Response response,
            ResponseObject<SizeDTO> responseSize)
        {
            _sizeConverter = new SizeConverter();
            _dbContext = dbContext;
            _tokenHelper = tokenHelper;
            _response = response;
            _responseSize = responseSize;
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
        private static void ValidateSize(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên kích cỡ không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giá trị kích cỡ không được để trống !");
            }
        }
        private async Task<Entities.Size> IsSizeExist(int sizeID)
        {
            var size = await _dbContext.Size.FirstOrDefaultAsync(x => x.ID == sizeID);
            return size ?? throw new CustomException(StatusCodes.Status404NotFound, "Kích cỡ không tồn tại !");
        }
        #endregion
        public async Task<Response> CreateSize(CreateSizeRequest request)
        {
            IsAdmin();
            ValidateSize(request.Name!, request.Value!);
            var size = new Entities.Size
            {
                Name = request.Name,
                Value = request.Value,
            };
            await _dbContext.Size.AddAsync(size);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Thêm kích cỡ thành công !");
        }

        public async Task<PageResult<SizeDTO>> GetAllSize(Pagination pagination)
        {
            IsAdmin();

            var query = _dbContext.Size.OrderByDescending(x => x.ID).AsQueryable();

            pagination.TotalCount = await query.CountAsync();
            var result = PageResult<Entities.Size>.ToPageResult(pagination, query);

            var list = result.ToList();

            return new PageResult<SizeDTO>(pagination, _sizeConverter.ListEntitySizeToDTO(result.ToList()));
        }

        public async Task<ResponseObject<SizeDTO>> GetSizeByID(int sizeID)
        {
            IsAdmin();
            var size = await IsSizeExist(sizeID);

            return _responseSize.ResponseSuccess("Thành công !", _sizeConverter.EntitySizeToDTO(size));
        }

        public async Task<Response> UpdateSize(int sizeID, UpdateSizeRequest request)
        {
            IsAdmin();
            ValidateSize(request.Name!, request.Value!);
            var size = await IsSizeExist(sizeID);

            size.Name = request.Name;
            size.Value = request.Value;
            size.UpdatedAt = DateTime.Now;

            _dbContext.Size.Update(size);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Cập nhật kích cỡ thành công !");
        }
    }
}
