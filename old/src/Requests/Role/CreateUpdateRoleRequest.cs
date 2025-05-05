using System.ComponentModel.DataAnnotations;

namespace Requests.Role
{
    public class CreateUpdateRoleRequest
    {
        [Required(ErrorMessage = "Tên quyền không được để trống !"), MaxLength(30, ErrorMessage = "Tên quyền không được quá 30 ký tự !")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tên hiển thị quyền không được để trống !"), MaxLength(30, ErrorMessage = "Tên hiển thị quyền không được quá 30 ký tự !")]
        public string DisplayName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mô tả quyền không được để trống !"), MaxLength(300, ErrorMessage = "Mô tả quyền không được quá 300 ký tự !")]
        public string Description { get; set; } = string.Empty;

        public List<int>? ListPermissionId { get; set; }
        public List<int>? ListMenuId { get; set; }
    }
}
