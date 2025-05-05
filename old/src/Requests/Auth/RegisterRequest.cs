using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Họ và tên không được để trống!")]
        [MaxLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự !")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống !")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng !")]
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự !")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống !")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự !")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ và số !")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giới tính không được để trống !")]
        [Range(0, 1, ErrorMessage = "Giới tính không hợp lệ! (0: Nam, 1: Nữ)")]
        public int Gender { get; set; }

        [DefaultValue("")]
        public string UserName { get; set; } = string.Empty;

        [DefaultValue("")]
        [MaxLength(255, ErrorMessage = "Đường dẫn ảnh đại diện không được vượt quá 255 ký tự!")]
        public string? Avatar { get; set; } = string.Empty;
    }
}
