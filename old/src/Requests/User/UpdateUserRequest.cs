using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.User
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Họ và tên không được để trống!")]
        [MaxLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự!")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự!")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(30, ErrorMessage = "Tên người dùng không được vượt quá 30 ký tự!")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống !")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự!")]
        [RegularExpression(@"(?:\+84|0084|0)[235789][0-9]{1,2}[0-9]{7}(?:[^\d]+|$)", ErrorMessage = "Không đúng định dạng số điện thoại!")]
        public string NumberPhone { get; set; } = string.Empty;


        [DefaultValue("")]
        [MaxLength(255, ErrorMessage = "Đường dẫn ảnh đại diện không được vượt quá 255 ký tự!")]
        public string? Avatar { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống!")]
        [Range(0, 1, ErrorMessage = "Giới tính không hợp lệ! (0: Nam, 1: Nữ)")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Trạng thái tài khoản không được để trống!")]
        [Range(0, 1, ErrorMessage = "Trạng thái không hợp lệ! (0: Không hoạt động, 1: Hoạt động)")]
        public int Status { get; set; }

        public List<int>? ListRoleId { get; set; }
    }
}
