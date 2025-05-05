using DTOs.User;
using Requests.User;
using Responses;

namespace Business.Interface
{
    public interface IUserService
    {
        public Task<PageResult<UserDTO>> GetAllUser(FilterUserRequest filter);
        public Task<ResponseObject<UserDTO>> GetUserById(int userId);
        public Task<ResponseObject<UserDTO>> GetFullDataUserById(int userId);
        public Task<ResponseText> CreateUser(CreateUserRequest request);
        public Task<ResponseText> UpdateUser(int userId, UpdateUserRequest request);
        public Task<ResponseText> DeleteUser(int userId);
    }
}
