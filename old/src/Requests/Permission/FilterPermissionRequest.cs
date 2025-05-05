using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.Permission
{
    public class FilterPermissionRequest
    {
        [Required(ErrorMessage = "Số lượng bản ghi không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng bản ghi không được nhỏ hơn 1 !")]
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Số trang không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số trang không được nhỏ hơn 1 !")]
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [MaxLength(100, ErrorMessage = "Tên quyền không được quá 100 ký tự !")]
        [DefaultValue(null)]
        public string? DisplayName { get; set; } = null;
    }
}
