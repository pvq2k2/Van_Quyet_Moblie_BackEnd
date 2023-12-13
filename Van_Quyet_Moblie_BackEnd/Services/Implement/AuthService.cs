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
using Van_Quyet_Moblie_BackEnd.Middleware;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Auth;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly AuthConverter _authConverter;
        private readonly AppDbContext _dbContext;
        private readonly ResponseObject<AccountDTO> _responseAccount;
        private readonly ResponseObject<AuthDTO> _responseAuth;
        private readonly IConfiguration _configuration;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly TokenHelper _tokenHelper;
        private readonly Response _response;

        public AuthService(AppDbContext dbContext,
            ResponseObject<AccountDTO> responseAccount,
            ResponseObject<AuthDTO> responseAuth,
            IConfiguration configuration,
            CloudinaryHelper cloudinaryHelper,
            TokenHelper tokenHelper,
            Response response)
        {
            _authConverter = new AuthConverter();
            _dbContext = dbContext;
            _responseAccount = responseAccount;
            _responseAuth = responseAuth;
            _configuration = configuration;
            _cloudinaryHelper = cloudinaryHelper;
            _tokenHelper = tokenHelper;
            _response = response;
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
                throw new Exception($"Lỗi khi gửi mail {ex.Message}");
            }
        }
        private string CreateAccessToken(Entities.Account account)
        {
            var claims = new List<Claim>
            {
                new Claim("ID", account.ID.ToString()),
                new Claim(ClaimTypes.Name, account.User!.FullName!),
                new Claim(ClaimTypes.Email, account.User!.Email!),
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
        public async Task<Response> VerifyEmail(string token)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(a => a.VerificationToken == token)
                ?? throw new CustomException(StatusCodes.Status400BadRequest, "Mã xác thực không hợp lệ !");
            account.Status = 2;
            account.VerifiedAt = DateTime.Now;

            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();
            return _response.ResponseSuccess("Xác thực thành công !");
        }
        public async Task<Response> Register(RegisterRequest request)
        {
            InputHelper.RegisterValidate(request);
            if (await _dbContext.Account.AnyAsync(x => x.UserName == request.UserName))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên tài khoản đã được sử dụng !");
            }
            if (await _dbContext.User.AnyAsync(x => x.Email == request.Email))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Email đã được sử dụng !");
            }
            if (await _dbContext.User.AnyAsync(x => x.NumberPhone == request.NumberPhone))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Số điện thoại đã được sử dụng !");
            }

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
                    FullName = InputHelper.NormalizeName(request.FullName!),
                    NumberPhone = request.NumberPhone,
                    Email = request.Email,
                    Gender = request.Gender,
                    Avatar = request.Gender == 1 ? "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127887/root/nam_pjwipr.jpg" : "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127885/root/nu_uqiotb.jpg",
                    AccountID = Account.ID
                };

                await _dbContext.User.AddAsync(User);
                await _dbContext.SaveChangesAsync();

                var EmailContent = new EmailFormat
                {
                    To = request.Email!,
                    Subject = "Xác thực tài khoản",
                    Body = EmailTemplate.MailTemplateString(User.FullName!, User.Email!,
                    "Vui lòng nhấp vào nút kích hoạt tài khoản để kích hoạt tài khoản !",
                    $"{_configuration.GetSection("AppSettings:BaseURLFE").Value!}/verify-account/{Account.VerificationToken}",
                    "Kích hoạt tài khoản")
                };
                SendEmail(EmailContent);

                await tran.CommitAsync();
                return _response.ResponseSuccess("Tạo tài khoản thành công !");
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseObject<AuthDTO>> ReNewToken(string refreshToken)
        {
            _tokenHelper.IsToken();

            var existingRefreshToken = await _dbContext.RefreshToken.FirstOrDefaultAsync(x => x.Token == refreshToken)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database");
            if (existingRefreshToken.ExpiredTime < DateTime.Now)
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Phiên đăng nhập đã hết hạn");
            }

            var account = _dbContext.Account
                .Include(x => x.User)
                .Include(x => x.Decentralization)
                .FirstOrDefault(x => x.ID == existingRefreshToken.AccountID)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại");

            var newAccessToken = CreateAccessToken(account);
            var newRefreshToken = GenerateRefreshToken(account.ID);
            existingRefreshToken.Token = newRefreshToken.Token;
            existingRefreshToken.CreatedAt = newRefreshToken.CreatedAt;
            existingRefreshToken.ExpiredTime = newRefreshToken.ExpiredTime;

            _dbContext.RefreshToken.Update(existingRefreshToken);
            await _dbContext.SaveChangesAsync();

            var responseAccount = new AuthDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                UserName = account.UserName,
                Avatar = account.User!.Avatar,
                Role = account.DecentralizationID
            };

            return _responseAuth.ResponseSuccess("Làm mới token thành công", responseAccount);
        }
        public async Task<ResponseObject<AuthDTO>> Login(LoginRequest request)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == request.Email)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !");
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ID == user.AccountID) 
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tài khoản hoặc mật khẩu không chính xác !");
            }
            if (account.VerifiedAt == null)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tài khoản chưa được xác thực !");
            }
            if (account.Status != (int)Status.Active)
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Tài khoản đã bị khóa !");
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

            var responseAccount = new AuthDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                UserName = currentAccount.UserName,
                Avatar = currentAccount.User!.Avatar,
                Role = account.DecentralizationID
            };

            return _responseAuth.ResponseSuccess("Đăng nhập thành công !", responseAccount);
        }
        public async Task<Response> ForgotPassword(string email)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == email)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !");
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
                $"{_configuration.GetSection("AppSettings:BaseURLFE").Value!}/reset-password/{account.ResetPasswordToken}",
                "Đổi mật khẩu")
            };
            SendEmail(EmailContent);

            return _response.ResponseSuccess("Gửi yêu cầu thành công !");
        }
        public async Task<Response> ResetPassword(ResetPasswordRequest request)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ResetPasswordToken == request.Token)
                ?? throw new CustomException(StatusCodes.Status400BadRequest, "Mã không hợp lệ !");
            if (account.ResetPasswordTokenExpiry < DateTime.Now)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Mã đã hết hạn !");
            }
            account.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            account.ResetPasswordToken = null;
            account.ResetPasswordTokenExpiry = null;

            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Đặt lại mật khẩu thành công !");
        }
        public async Task<Response> ChangePassword(ChangePasswordRequest request)
        {
            _tokenHelper.IsToken();
            var accountID = _tokenHelper.GetUserID();
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ID == accountID)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !");

            if (!BCrypt.Net.BCrypt.Verify(request.OddPassword, account.Password))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Mật khẩu cũ không đúng !");
            }

            account.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Thay đổi mật khẩu thành công !");
        }
        public async Task<PageResult<AccountDTO>> GetAllAccount(Pagination pagination)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();

            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }

            var query = _dbContext.Account.Include(x => x.User).Include(x => x.Decentralization).OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Entities.Account>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<AccountDTO>(pagination, _authConverter.ListEntityAccountToDTO(result.ToList()));
        }
        public async Task<ResponseObject<AccountDTO>> GetAccountByID(int accountID)
        {
            var account = await _dbContext.Account.Include(x => x.User).Include(x => x.Decentralization).FirstOrDefaultAsync(x => x.ID == accountID)
                ?? throw new CustomException(StatusCodes.Status404NotFound, $"Tài khoản có ID {accountID} không tồn tại !");
            return _responseAccount.ResponseSuccess("Thành Công !", _authConverter.EntityAccountToDTO(account));
        }
        public async Task<ResponseObject<AuthDTO>> ChangeInformation(ChangeInformationRequest request)
        {
            InputHelper.ChangeInformationValidate(request);

            _tokenHelper.IsToken();
            var accountID = _tokenHelper.GetUserID();
            var account = await _dbContext.Account.Include(x => x.User).Include(x => x.Decentralization).FirstOrDefaultAsync(x => x.ID == accountID)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !");

            if (await _dbContext.User.AnyAsync(x => x.Email == request.Email) && request.Email != account.User!.Email)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Email đã được sử dụng !");
            }
            if (await _dbContext.User.AnyAsync(x => x.NumberPhone == request.NumberPhone) && request.NumberPhone != account.User!.NumberPhone)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Số điện thoại đã được sử dụng !");
            }
            string avatar;
            if (request.Avatar != null)
            {
                InputHelper.IsImage(request.Avatar!);
                avatar = await _cloudinaryHelper.UploadImage(request.Avatar!, "van-quyet-mobile/user", "avatar");
                await _cloudinaryHelper.DeleteImageByUrl(account.User!.Avatar!);
            }
            else
            {
                string defaultAvatarMale = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127887/root/nam_pjwipr.jpg";
                string defaultAvatarFamele = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127885/root/nu_uqiotb.jpg";
                if (account.User?.Avatar != defaultAvatarMale && account.User?.Avatar != defaultAvatarFamele)
                {
                    avatar = account.User?.Avatar!;
                }
                else
                {
                    avatar = request.Gender == 1 ? defaultAvatarMale : defaultAvatarFamele;
                }
            }
            account.User!.FullName = request.FullName;
            account.User.NumberPhone = request.NumberPhone;
            account.User.Email = request.Email;
            account.User.Avatar = avatar;
            account.User.Gender = request.Gender;
            account.User.Address = request.Address;
            account.User.UpdatedAt = DateTime.Now;
            account.UpdatedAt = DateTime.Now;

            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();

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
            var responseAccount = new AuthDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                UserName = currentAccount.UserName,
                Avatar = currentAccount.User!.Avatar,
                Role = account.DecentralizationID
            };

            return _responseAuth.ResponseSuccess("Cập nhật thông tin thành công !", responseAccount);
        }
        public async Task<Response> ChangeStatus(int accountID, int status)
        {
            _tokenHelper.IsToken();
            string role = _tokenHelper.GetRole();

            if (role != "Admin")
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Không có quyền !");
            }
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.ID == accountID)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Tài khoản không tồn tại !");

            if (status != 1 && status != 2)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Trạng thái không đúng!");
            }

            account.Status = status;
            _dbContext.Account.Update(account);
            await _dbContext.SaveChangesAsync();

            return _response.ResponseSuccess("Chuyển trạng thái thành công !");
        }
    }
}
