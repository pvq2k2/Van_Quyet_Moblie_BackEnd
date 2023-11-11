using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IAuthService
    {
        public Task<ResponseObject<AccountDTO>> Register(RegisterRequest request);
        public Task<ResponseObject<TokenDTO>> Login(LoginRequest request);
        public ResponseObject<TokenDTO> ReNewToken(string refreshToken);
        public Task<ResponseObject<string>> VerifyEmail(string token);
        public Task<ResponseObject<string>> ForgotPassword(string email);
        public Task<ResponseObject<string>> ResetPassword(ResetPasswordRequest request);
        public Task<ResponseObject<string>> ChangePassword(ChangePasswordRequest request);
        public Task<PageResult<AccountDTO>> GetAllAccount(Pagination pagination);
        public Task<ResponseObject<AccountDTO>> GetAccountByID(int accountID);
        public Task<ResponseObject<AccountDTO>> ChangeInformation(ChangeInformationRequest request);
        public Task<ResponseObject<string>> ChangeStatus(int accountID, int status);
    }
}
