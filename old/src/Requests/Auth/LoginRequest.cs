using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được để trống !")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng !")]
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự !")]
        [DefaultValue("quyet@gmail.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống !")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự !")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ và số.")]
        [DefaultValue("Q1234567")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "ReCaptcha không được để trống !")]
        public string? ReCaptchaToken { get; set; }
    }
}
