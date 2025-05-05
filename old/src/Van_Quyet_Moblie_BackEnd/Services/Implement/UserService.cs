using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.DataContext;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.User;
using Van_Quyet_Moblie_BackEnd.Handle.Request.UserRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly TokenHelper _tokenHelper;
        private readonly Response _response;
        private readonly UserConverter _userConverter;
        private readonly ResponseObject<GetUpdateUserDTO> _responseObject;

        public UserService(AppDbContext dbContext,
            TokenHelper tokenHelper,
            Response response,
            ResponseObject<GetUpdateUserDTO> responseObject)
        {
            _userConverter = new UserConverter();
            _dbContext = dbContext;
            _tokenHelper = tokenHelper;
            _response = response;
            _responseObject = responseObject;
        }
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
        private static void ValidateUser(UpdateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên họ và tên không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(request.NumberPhone))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Số điện thoại không được để trống !");
            }
            if (!InputHelper.RegexPhoneNumber(request.NumberPhone!))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Số điện thoại không đúng định dạng !");
            }
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Email không được để trống !");
            }
            if (!InputHelper.RegexEmail(request.Email!))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Email không đúng định dạng !");
            }
        }
        private async Task<User> IsUserExist(int userID)
        {
            var user = await _dbContext.User.Include(x => x.Account).FirstOrDefaultAsync(x => x.ID == userID);
            return user ?? throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại !");
        }
        #endregion
        public async Task<PageResult<UserDTO>> GetAllUser(Pagination pagination)
        {
            IsAdmin();
            var query = _dbContext.User.Include(x => x.Account).AsQueryable();

            pagination.TotalCount = await query.CountAsync();
            var result = PageResult<User>.ToPageResult(pagination, query);

            return new PageResult<UserDTO>(pagination, _userConverter.ListEntityUserToDTO(result.ToList()));
        }

        public async Task<ResponseObject<GetUpdateUserDTO>> GetUpdateUserByID(int userID)
        {
            IsAdmin();
            var user = await IsUserExist(userID);
            var address = await _dbContext.Address.FirstOrDefaultAsync(x => x.UserID == userID);

            var DTO = new GetUpdateUserDTO() {
                ID = user.ID,
                FullName = user.FullName,
                NumberPhone = user.NumberPhone,
                Gender = user.Gender,
                Email = user.Email,
                Status = user.Account!.Status,
                Address = address,
            };

            return _responseObject.ResponseSuccess("Thành công !", DTO);
        }

        public async Task<Response> UpdateUser(int userID, UpdateUserRequest request)
        {
            IsAdmin();
            ValidateUser(request);
            var user = await IsUserExist(userID);
            var address = await _dbContext.Address.FirstOrDefaultAsync(x => x.UserID == userID);

            if (request.DetailAddress != "" &&
                request.ProvinceID != 0 &&
                request.DistrictID != 0 &&
                request.WardID != 0)
            {
                if (address != null)
                {
                    address.WardID = request.WardID;
                    address.ProvinceID = request.ProvinceID;
                    address.DistrictID = request.DistrictID;
                    address.DetailAddress = request.DetailAddress;
                    address.UpdatedAt = DateTime.Now;

                    _dbContext.Address.Update(address);
                    await _dbContext.SaveChangesAsync();
                } else
                {
                    var newAddress = new Address()
                    {
                        WardID = request.WardID,
                        ProvinceID = request.ProvinceID,
                        DistrictID = request.DistrictID,
                        DetailAddress = request.DetailAddress,
                        UserID = userID
                    };

                    await _dbContext.Address.AddAsync(newAddress);
                    await _dbContext.SaveChangesAsync();
                }
            }

            user.FullName = request.FullName;
            user.NumberPhone = request.NumberPhone;
            user.Gender = request.Gender;
            user.Email = request.Email;
            user.Account!.Status = request.Status;

            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Cập nhật người dùng thành công !");
        }
    }
}
