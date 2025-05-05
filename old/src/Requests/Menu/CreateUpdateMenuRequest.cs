using System.ComponentModel.DataAnnotations;

namespace Requests.Menu
{
    public class CreateUpdateMenuRequest
    {
        [Required(ErrorMessage = "Tên menu không được để trống !"), MaxLength(30, ErrorMessage = "Tên menu không được quá 30 ký tự !")]
        public string DisplayName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Url không được để trống !"), Url(ErrorMessage = "Url không đúng định dạng !")]
        public string Url { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tên icon không được để trống !"), MaxLength(30, ErrorMessage = "Tên icon không được quá 30 ký tự !")]
        public string IconName { get; set; } = string.Empty;
    }
}
