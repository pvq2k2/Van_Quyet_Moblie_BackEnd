using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests.Permission
{
    public class CreateUpdatePermissionRequest
    {
        [Required(ErrorMessage = "Tên quyền không được để trống !"),
            MaxLength(100, ErrorMessage = "Tên quyền không được quá 100 ký tự !"),
            RegularExpression(@"^[a-zA-Z0-9]+\.[a-zA-Z0-9]+$", ErrorMessage = "Tên phải phân tách bằng dấu chấm (Group.Action)")]
        [DefaultValue("Group.Action")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tên hiển thị quyền không được để trống !"), MaxLength(100, ErrorMessage = "Tên hiển thị quyền không được quá 100 ký tự !")]
        public string DisplayName { get; set; } = string.Empty;
        [DefaultValue("")]
        [MaxLength(100, ErrorMessage = "Tên nhóm quyền không được quá 100 ký tự !")]
        public string GroupName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mô tả quyền không được để trống !"), MaxLength(300, ErrorMessage = "Mô tả quyền không được quá 300 ký tự !")]
        public string Description { get; set; } = string.Empty;
    }
}
