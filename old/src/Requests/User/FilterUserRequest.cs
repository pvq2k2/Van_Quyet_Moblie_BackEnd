using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.User
{
    public class FilterUserRequest
    {
        [Required(ErrorMessage = "Số lượng bản ghi không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng bản ghi không được nhỏ hơn 1 !")]
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Số trang không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số trang không được nhỏ hơn 1 !")]
        [DefaultValue(1)]
        public int PageNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự!")]
        [DefaultValue(null)]
        public string? FullName { get; set; } = null;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự!")]
        [DefaultValue(null)]
        public string? Email { get; set; } = null;

        [MaxLength(30, ErrorMessage = "Tên người dùng không được vượt quá 30 ký tự!")]
        [DefaultValue(null)]
        public string? UserName { get; set; } = null;

        [MaxLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự!")]
        [RegularExpression(@"(?:\+84|0084|0)[235789][0-9]{1,2}[0-9]{7}(?:[^\d]+|$)", ErrorMessage = "Không đúng định dạng số điện thoại!")]
        [DefaultValue(null)]
        public string? NumberPhone { get; set; } = null;

        [Range(0, 1, ErrorMessage = "Giới tính không hợp lệ! (0: Nam, 1: Nữ)")]
        [DefaultValue(null)]
        public int? Gender { get; set; } = null;

        [Range(0, 1, ErrorMessage = "Trạng thái không hợp lệ! (0: Không hoạt động, 1: Hoạt động)")]
        [DefaultValue(null)]
        public int? Status { get; set; } = null;
    }
}
