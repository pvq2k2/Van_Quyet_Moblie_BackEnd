using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.Role
{
    public class FilterRoleRequest
    {
        [Required(ErrorMessage = "Số lượng bản ghi không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng bản ghi không được nhỏ hơn 1 !")]
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Số trang không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số trang không được nhỏ hơn 1 !")]
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [MaxLength(30, ErrorMessage = "Tên quyền không được quá 30 ký tự !")]
        [DefaultValue(null)]
        public string? DisplayName { get; set; } = null;
    }
}
