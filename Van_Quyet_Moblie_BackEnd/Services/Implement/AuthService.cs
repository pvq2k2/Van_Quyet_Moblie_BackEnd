using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;
using Van_Quyet_Moblie_BackEnd.Enums;
using QuanLyTrungTam_API.Helper;
using CloudinaryDotNet;
using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.DataContext;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly AuthConverter _authConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<AccountDTO> _responseAccount;
        private readonly ResponseObject<TokenDTO> _responseAuth;
        private readonly IConfiguration _configuration;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly TokenHelper _tokenHelper;

        public AuthService(AppDbContext dbContext,
            ResponseObject<AccountDTO> responseAccount,
            ResponseObject<TokenDTO> responseAuth,
            IConfiguration configuration,
            CloudinaryHelper cloudinaryHelper,
            TokenHelper tokenHelper)
        {
            _authConverter = new AuthConverter();
            _dbContext = dbContext;
            _responseAccount = responseAccount;
            _responseAuth = responseAuth;
            _configuration = configuration;
            _cloudinaryHelper = cloudinaryHelper;
            _tokenHelper = tokenHelper;
        }
        #region Private Function
        private static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private void SendEmail(EmailFormat request)
        {
            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(_configuration.GetSection("AppSettings:EmailSettings:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("AppSettings:EmailSettings:EmailUserName").Value, _configuration.GetSection("AppSettings:EmailSettings:EmailPassword").Value);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse($"{_configuration.GetSection("AppSettings:ShopName").Value} <{_configuration.GetSection("AppSettings:EmailSettings:EmailUserName").Value}>"));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi khi gửi mail {ex.Message}");
            }
        }

        private string CreateAccessToken(Entities.Account account)
        {
            var claims = new List<Claim>
            {
                new Claim("ID", account.ID.ToString()),
                new Claim(ClaimTypes.Name, account.User!.FullName!),
                new Claim(ClaimTypes.Email, account.User!.Email!),
                new Claim("Avatar", account.User!.Avatar!),
                new Claim("RoleID", account.DecentralizationID.ToString()),
                new Claim(ClaimTypes.Role, account.Decentralization!.Name!),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:AccessTokenSecret").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static RefreshToken GenerateRefreshToken(int AccountID)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                AccountID = AccountID,
                CreatedAt = DateTime.Now,
                ExpiredTime = DateTime.Now.AddDays(5)
            };

            return refreshToken;
        }


        #endregion


        public async Task<ResponseObject<string>> VerifyEmail(string token)
        {
            var response = new ResponseObject<string>();
            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.VerificationToken == token);
            if (account == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Mã xác thực không hợp lệ !", null!);
            }
            account.Status = 1;
            account.VerifiedAt = DateTime.Now;

            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();
            return response.ResponseSuccess("Xác thực thành công !", null!);
        }

        public async Task<ResponseObject<AccountDTO>> Register(RegisterRequest request)
        {
            try
            {
                InputHelper.RegisterValidate(request);
                //InputHelper.IsImage(request.Avatar!);
                if (await _dbContext.Account.AnyAsync(x => x.UserName == request.UserName))
                {
                    return _responseAccount.ResponseError(StatusCodes.Status400BadRequest, "Tên tài khoản đã được sử dụng !", null!);
                }
                if (await _dbContext.User.AnyAsync(x => x.Email == request.Email))
                {
                    return _responseAccount.ResponseError(StatusCodes.Status400BadRequest, "Email đã được sử dụng !", null!);
                }
                if (await _dbContext.User.AnyAsync(x => x.Phone == request.Phone))
                {
                    return _responseAccount.ResponseError(StatusCodes.Status400BadRequest, "Số điện thoại đã được sử dụng !", null!);
                }
                //string avatar = await _cloudinaryHelper.UploadImage(request.Avatar!, "van-quyet-mobile/user", "avatar");

                using var tran = _dbContext.Database.BeginTransaction();
                try
                {
                    var Account = new Entities.Account
                    {
                        UserName = request.UserName,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        Status = (int)Status.InActive,
                        DecentralizationID = (int)Enums.Decentralization.User,
                        VerificationToken = CreateRandomToken()
                    };
                    await _dbContext.Account.AddAsync(Account);
                    await _dbContext.SaveChangesAsync();

                    var User = new User
                    {
                        FullName = InputHelper.NormalizeName(request.FullName),
                        Phone = request.Phone,
                        Email = request.Email,
                        Avatar = null!,
                        Address = request.Address,
                        AccountID = Account.ID
                    };

                    await _dbContext.User.AddAsync(User);
                    await _dbContext.SaveChangesAsync();

                    var EmailContent = new EmailFormat
                    {
                        To = request.Email,
                        Subject = "Xác thực tài khoản",
                        Body = EmailTemplate.MailTemplateString(User.FullName!, User.Email!,
                        "Vui lòng nhấp vào nút kích hoạt tài khoản để kích hoạt tài khoản !",
                        $"https://localhost:7299/api/Auth/Verify/{Account.VerificationToken}",
                        "Kích hoạt tài khoản")
                    };
                    SendEmail(EmailContent);


                    var currentAccount = await _dbContext.Account
                        .Include(x => x.User)
                        .Include(x => x.Decentralization)
                        .FirstOrDefaultAsync(x => x.ID == Account.ID);
                    await tran.CommitAsync();
                    return _responseAccount.ResponseSuccess("Tạo tài khoản thành công !", _authConverter.EntityAccountToDTO(currentAccount!));
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return _responseAccount.ResponseError(StatusCodes.Status400BadRequest, ex.Message, null!);
            }
        }

        public ResponseObject<TokenDTO> ReNewToken(string refreshToken)
        {
            try
            {
                _tokenHelper.IsToken();

                var existingRefreshToken = _dbContext.RefreshToken.FirstOrDefault(x => x.Token == refreshToken);

                if (existingRefreshToken == null)
                {
                    return _responseAuth.ResponseError(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null!);
                }

                if (existingRefreshToken.ExpiredTime < DateTime.Now)
                {
                    return _responseAuth.ResponseError(StatusCodes.Status401Unauthorized, "Phiên đăng nhập đã hết hạn", null!);
                }

                var account = _dbContext.Account
                    .Include(x => x.User)
                    .Include(x => x.Decentralization)
                    .FirstOrDefault(x => x.ID == existingRefreshToken.AccountID);

                if (account == null)
                {
                    return _responseAuth.ResponseError(StatusCodes.Status404NotFound, "Tài khoản không tồn tại", null!);
                }

                var newAccessToken = CreateAccessToken(account);
                var newRefreshToken = GenerateRefreshToken(account.ID);
                existingRefreshToken.Token = newRefreshToken.Token;
                existingRefreshToken.CreatedAt = newRefreshToken.CreatedAt;
                existingRefreshToken.ExpiredTime = newRefreshToken.ExpiredTime;

                _dbContext.RefreshToken.Update(existingRefreshToken);
                _dbContext.SaveChanges();

                return _responseAuth.ResponseSuccess("Làm mới token thành công", new TokenDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token });
            }
            catch (Exception ex)
            {
                return _responseAuth.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }

        public async Task<ResponseObject<TokenDTO>> Login(LoginRequest request)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (account == null || account.Status == (int)Status.InActive)
            {
                return _responseAuth.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản không tồn tại !", null!);
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
            {
                return _responseAuth.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản hoặc mật khẩu không chính xác !", null!);
            }
            if (account.VerifiedAt == null)
            {
                return _responseAuth.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được xác thực !", null!);
            }
            var currentAccount = await _dbContext.Account
                .Include(x => x.User)
                .Include(x => x.Decentralization)
                .FirstOrDefaultAsync(x => x.ID == account.ID);

            string accessToken = CreateAccessToken(currentAccount!);

            var refreshToken = GenerateRefreshToken(currentAccount!.ID);

            var myRefreshToken = await _dbContext.RefreshToken.FirstOrDefaultAsync(x => x.AccountID == currentAccount.ID);

            if (myRefreshToken == null)
            {
                await _dbContext.RefreshToken.AddAsync(refreshToken);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                myRefreshToken.Token = refreshToken.Token;
                myRefreshToken.CreatedAt = refreshToken.CreatedAt;
                myRefreshToken.ExpiredTime = refreshToken.ExpiredTime;

                _dbContext.RefreshToken.Update(myRefreshToken);
                await _dbContext.SaveChangesAsync();
            }

            return _responseAuth.ResponseSuccess("Đăng nhập thành công !", new TokenDTO { AccessToken = accessToken, RefreshToken = refreshToken.Token });
        }

        public async Task<ResponseObject<string>> ForgotPassword(string email)
        {
            var response = new ResponseObject<string>();
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Người dùng không tồn tại !", null!);
            }
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ID == user.AccountID);
            account!.ResetPasswordToken = CreateRandomToken();
            account.ResetPasswordTokenExpiry = DateTime.Now.AddHours(5);

            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();
            var EmailContent = new EmailFormat
            {
                To = email,
                Subject = "Đặt lại mật khẩu",
                Body = EmailTemplate.MailTemplateString(user.FullName!, user.Email!,
                "Vui lòng nhấp vào nút đổi mật khẩu để đổi mật khẩu !. \nMã chỉ có hiệu lực trong vòng 5 giờ tính từ khi gửi yêu cầu !",
                $"https://localhost:7299/api/Auth/change-password/{account.ResetPasswordToken}",
                "Đổi mật khẩu")
            };
            SendEmail(EmailContent);

            return response.ResponseSuccess("Gửi yêu cầu thành công, vui lòng kiểm tra hộp thư của bạn !", null!);
        }

        public async Task<ResponseObject<string>> ResetPassword(ResetPasswordRequest request)
        {
            var response = new ResponseObject<string>();
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ResetPasswordToken == request.Token);
            if (account == null)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Mã không hợp lệ !", null!);
            }
            if (account.ResetPasswordTokenExpiry < DateTime.Now)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Mã đã hết hạn !", null!);
            }
            account.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            account.ResetPasswordToken = null;
            account.ResetPasswordTokenExpiry = null;

            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Đổi mật khẩu thành công !", null!);
        }

        public async Task<ResponseObject<string>> ChangePassword(ChangePasswordRequest request)
        {
            var response = new ResponseObject<string>();
            try
            {
                _tokenHelper.IsToken();
                var accountID = _tokenHelper.GetUserID();
                var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ID == accountID);

                if (account == null)
                {
                    return response.ResponseError(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !", null!);
                }

                if (!BCrypt.Net.BCrypt.Verify(request.OddPassword, account.Password))
                {
                    return response.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu cũ không đúng !", null!);
                }

                account.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                _dbContext.Account.Update(account);
                await _dbContext.SaveChangesAsync();

                return response.ResponseSuccess("Thay đổi mật khẩu thành công !", null!);
            }
            catch (Exception ex)
            {
                return response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }

        public async Task<PageResult<AccountDTO>> GetAllAccount(Pagination pagination)
        {
            var query = _dbContext.Account.Include(x => x.User).Include(x => x.Decentralization).OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Entities.Account>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<AccountDTO>(pagination, _authConverter.ListEntityAccountToDTO(result.ToList()));
        }

        public async Task<ResponseObject<AccountDTO>> GetAccountByID(int accountID)
        {
            var account = await _dbContext.Account.Include(x => x.User).Include(x => x.Decentralization).FirstOrDefaultAsync(x => x.ID == accountID);
            if (account == null)
            {
                return _responseAccount.ResponseError(StatusCodes.Status404NotFound, $"Tài khoản có ID {accountID} không tồn tại !", null!);
            }
            return _responseAccount.ResponseSuccess("Thành Công !", _authConverter.EntityAccountToDTO(account));
        }

        public async Task<ResponseObject<AccountDTO>> ChangeInformation(ChangeInformationRequest request)
        {
            try
            {
                InputHelper.ChangeInformationValidate(request);

                _tokenHelper.IsToken();
                var accountID = _tokenHelper.GetUserID();
                var account = await _dbContext.Account.Include(x => x.User).Include(x => x.Decentralization).FirstOrDefaultAsync(x => x.ID == accountID);

                if (account == null)
                {
                    return _responseAccount.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy tài khoản !", null!);
                }

                if (await _dbContext.User.AnyAsync(x => x.Email == request.Email) && request.Email != account.User!.Email)
                {
                    return _responseAccount.ResponseError(StatusCodes.Status400BadRequest, "Email đã được sử dụng !", null!);
                }
                if (await _dbContext.User.AnyAsync(x => x.Phone == request.Phone) && request.Phone != account.User!.Phone)
                {
                    return _responseAccount.ResponseError(StatusCodes.Status400BadRequest, "Số điện thoại đã được sử dụng !", null!);
                }
                string avatar;
                if (request.Avatar != null)
                {
                    InputHelper.IsImage(request.Avatar!);
                    avatar = await _cloudinaryHelper.UploadImage(request.Avatar!, "van-quyet-mobile/user", "avatar");
                    await _cloudinaryHelper.DeleteImageByUrl(account.User!.Avatar);
                }
                else
                {
                    avatar = account.User!.Avatar;
                }
                account.User!.FullName = request.FullName;
                account.User.Phone = request.Phone;
                account.User.Email = request.Email;
                account.User.Avatar = avatar;
                account.User.Address = request.Address;
                account.User.UpdatedAt = DateTime.Now;
                account.UpdatedAt = DateTime.Now;

                _dbContext.Account.Update(account);
                await _dbContext.SaveChangesAsync();

                return _responseAccount.ResponseSuccess("Cập nhật thông tin thành công !", _authConverter.EntityAccountToDTO(account));
            }
            catch (Exception ex)
            {
                return _responseAccount.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }

        public async Task<ResponseObject<string>> ChangeStatus(int accountID, int status)
        {
            var response = new ResponseObject<string>();
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ID == accountID);

            if (account == null)
            {
                return response.ResponseError(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !", null!);
            }

            if (status != 1 || status != 2)
            {
                return response.ResponseError(StatusCodes.Status400BadRequest, "Trạng thái không đúng!", null!);
            }

            account.Status = status;
            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();

            return response.ResponseSuccess("Chuyển trạng thái thành công !", null!);
        }
    }
}
