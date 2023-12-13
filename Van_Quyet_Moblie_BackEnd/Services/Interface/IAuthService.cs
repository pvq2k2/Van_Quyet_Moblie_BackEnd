using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Auth;
using Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IAuthService
    {
        public Task<Response> Register(RegisterRequest request);
        public Task<ResponseObject<AuthDTO>> Login(LoginRequest request);
        public Task<ResponseObject<AuthDTO>> ReNewToken(string refreshToken);
        public Task<Response> VerifyEmail(string token);
        public Task<Response> ForgotPassword(string email);
        public Task<Response> ResetPassword(ResetPasswordRequest request);
        public Task<Response> ChangePassword(ChangePasswordRequest request);
        public Task<PageResult<AccountDTO>> GetAllAccount(Pagination pagination);
        public Task<ResponseObject<AccountDTO>> GetAccountByID(int accountID);
        public Task<ResponseObject<AuthDTO>> ChangeInformation(ChangeInformationRequest request);
        public Task<Response> ChangeStatus(int accountID, int status);
    }
}
