using Van_Quyet_Moblie_BackEnd.Handle.DTOs.User;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.UserRequest;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IUserService
    {
        public Task<PageResult<UserDTO>> GetAllUser(Pagination pagination);
        public Task<ResponseObject<GetUpdateUserDTO>> GetUpdateUserByID(int userID);
        public Task<Response> UpdateUser(int userID, UpdateUserRequest request);
    }
}
