namespace Application.Auth.Dto
{
    public class AuthResetPasswordDto
    {
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
