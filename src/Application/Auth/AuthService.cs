using Application.Auth.Dto;
using Application.Common.JwtToken;
using Infrastructure.Data;
using Application.Common.ExceptionHandling.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Google.Apis.Auth;
using Application.Common.Email;
using Application.Common.DeviceInfoProvider;

namespace Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly IDeviceInfoProvider _deviceInfoProvider;

        public AuthService(AppDbContext appDbContext, IJwtTokenService jwtTokenService, IEmailService emailService, IDeviceInfoProvider deviceInfoProvider)
        {
            _dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _deviceInfoProvider = deviceInfoProvider ?? throw new ArgumentNullException(nameof(deviceInfoProvider));
        }

        public async Task<AuthDto> Login(AuthLoginDto request)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new UnauthorizedAccessException("Tài khoản hoặc mật khẩu không chính xác.");

            if (user.VerifiedAt == null)
                throw new FriendlyException("Tài khoản chưa được xác thực email.", 401,
                new { needVerifyOtp = true });
            if (user.Status == 0)
                throw new UnauthorizedAccessException("Tài khoản đã bị khóa vui lòng liên hệ quản trị viên.");

            await _dbContext.Entry(user).Reference(u => u.Role).LoadAsync();
            await _dbContext.Entry(user.Role!).Collection(r => r.ListRolePermission!).Query().Include(rp => rp.Permission).LoadAsync();

            var permissions = user.Role!.ListRolePermission!
                .Select(rp => rp.Permission!.Name)
                .ToList();

            var (accessToken, refreshToken) = _jwtTokenService.GenerateTokens(user, permissions);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();

            // Gửi email cảnh báo bảo mật
            string deviceInfo = _deviceInfoProvider.GetDeviceInfo();
            await _emailService.SendEmailAsync(user.Email, "Cảnh báo bảo mật",
                EmailTemplate.SecurityAlertEmail(user.UserName, user.Email, deviceInfo));
            return new AuthDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Permissions = permissions
            };
        }

        public async Task<AuthDto> LoginWithGoogle(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var email = payload.Email;

            // Tìm user theo email
            var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Đăng ký mới nếu chưa có
                user = new User
                {
                    UserName = payload.Name,
                    Email = payload.Email,
                    VerifiedAt = DateTime.UtcNow,
                    Password = "", // không cần mật khẩu
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            if (user.Status == 0)
                throw new UnauthorizedAccessException("Tài khoản đã bị khóa vui lòng liên hệ quản trị viên.");

            await _dbContext.Entry(user).Reference(u => u.Role).LoadAsync();
            await _dbContext.Entry(user.Role!).Collection(r => r.ListRolePermission!).Query().Include(rp => rp.Permission).LoadAsync();

            var permissions = user.Role!.ListRolePermission!
                .Select(rp => rp.Permission!.Name)
                .ToList();

            var (accessToken, refreshToken) = _jwtTokenService.GenerateTokens(user, permissions);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();

            // Gửi email cảnh báo bảo mật
            string deviceInfo = _deviceInfoProvider.GetDeviceInfo();
            await _emailService.SendEmailAsync(user.Email, "Cảnh báo bảo mật",
                EmailTemplate.SecurityAlertEmail(user.UserName, user.Email, deviceInfo));
            return new AuthDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Permissions = permissions
            };
        }

        public async Task<AuthDto> RefreshToken(string refreshToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token không hợp lệ hoặc đã hết hạn.");

            await _dbContext.Entry(user).Reference(u => u.Role).LoadAsync();
            await _dbContext.Entry(user.Role!).Collection(r => r.ListRolePermission).Query().Include(rp => rp.Permission).LoadAsync();

            var permissions = user.Role!.ListRolePermission!
                .Select(rp => rp.Permission!.Name)
                .ToList();

            var (newAccessToken, newRefreshToken) = _jwtTokenService.GenerateTokens(user, permissions);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();

            return new AuthDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Permissions = permissions
            };
        }

        public async Task Logout(string refreshToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangePassword(int userId, AuthChangePasswordDto request)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("Không tìm thấy người dùng.");

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "oldPassword", new[] { "Mật khẩu cũ không chính xác." } }
                };

                throw new ValidationException(errors);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AuthUserDto?> GetCurrentUserInfo(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new AuthUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleName = user.Role?.Name
            };
        }

        public async Task Register(AuthRegiterDto request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email || u.UserName == request.UserName))
                throw new InvalidOperationException("Email hoặc tên đăng nhập đã tồn tại.");

            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(10);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = 1,
                OtpCode = otp,
                OtpCodeExpiry = expiry
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Chào mừng bạn đến với Văn Quyết Mobile",
                EmailTemplate.WelcomeEmail(user.UserName, user.Email));
            // Gửi email chứa mã OTP (otp) tới user.Email
            await _emailService.SendEmailAsync(user.Email, $"Mã OTP của bạn để xác thực là: {otp}", EmailTemplate.OtpEmail(user.UserName, user.Email, otp));
        }

        public async Task ResendOtp(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.VerifiedAt != null)
                throw new InvalidOperationException("Không thể gửi lại OTP.");

            var newOtp = new Random().Next(100000, 999999).ToString();

            user.OtpCode = newOtp;
            user.OtpCodeExpiry = DateTime.UtcNow.AddMinutes(10);
            user.OtpFailedCount = 0; // Reset số lần sai

            await _dbContext.SaveChangesAsync();

            //Gửi OTP qua email
            await _emailService.SendEmailAsync(user.Email, $"Mã OTP của bạn để xác thực là: {newOtp}", EmailTemplate.OtpEmail(user.UserName, user.Email, newOtp));
        }

        public async Task VerifyEmailOtp(AuthVerifyEmailOtpDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || user.VerifiedAt != null)
                throw new InvalidOperationException("Tài khoản không hợp lệ hoặc đã được xác thực.");

            if (user.OtpCodeExpiry < DateTime.UtcNow)
                throw new InvalidOperationException("Mã OTP đã hết hạn. Vui lòng yêu cầu mã mới.");

            if (user.OtpCode != request.OtpCode)
            {
                user.OtpFailedCount++;

                if (user.OtpFailedCount >= 3)
                {
                    user.OtpCode = null;
                    user.OtpCodeExpiry = null;
                    user.OtpFailedCount = 0;
                    await _dbContext.SaveChangesAsync();

                    throw new FriendlyException("Bạn đã nhập sai quá 3 lần. Vui lòng yêu cầu mã OTP mới.",
                            400,
                            new { needResend = true, failedAttempts = 3, maxAttempts = 3 });
                }

                await _dbContext.SaveChangesAsync();
                throw new FriendlyException($"Mã OTP không đúng. Bạn còn {3 - user.OtpFailedCount} lần thử.",
                            400,
                            new { failedAttempts = user.OtpFailedCount, maxAttempts = 3 });
            }

            // Đúng OTP
            user.VerifiedAt = DateTime.UtcNow;
            user.OtpCode = null;
            user.OtpCodeExpiry = null;
            user.OtpFailedCount = 0;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.VerifiedAt == null)
                throw new FriendlyException("Tài khoản không tồn tại hoặc chưa được xác thực.", 404);

            var token = Guid.NewGuid().ToString("N");
            var expiry = DateTime.UtcNow.AddMinutes(15);

            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = expiry;

            await _dbContext.SaveChangesAsync();

            var resetLink = $"https://yourdomain.com/reset-password?token={token}"; // link front-end reset
            var htmlBody = EmailTemplate.ForgotPasswordEmail(user.UserName, user.Email, resetLink);

            await _emailService.SendEmailAsync(user.Email, "Yêu cầu đặt lại mật khẩu", htmlBody);
        }
        public async Task ResetPassword(AuthResetPasswordDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.ResetPasswordToken == request.Token && u.ResetPasswordTokenExpiry > DateTime.UtcNow);

            if (user == null)
                throw new FriendlyException("Token không hợp lệ hoặc đã hết hạn.", 400);

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            await _dbContext.SaveChangesAsync();
        }

    }
}
