using System.ComponentModel.DataAnnotations;

namespace Requests.User
{
    public class CreateUserRequest : UpdateUserRequest
    {
        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ và số!")]
        public string Password { get; set; } = string.Empty;
    }
}
