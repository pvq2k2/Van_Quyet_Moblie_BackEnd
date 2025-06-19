namespace Application.Auth.Dto
{
    public class AuthVerifyEmailOtpDto
    {
        public required string Email { get; set; }
        public required string OtpCode { get; set; }
    }
}
