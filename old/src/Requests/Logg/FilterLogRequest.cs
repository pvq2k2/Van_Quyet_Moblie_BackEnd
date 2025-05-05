using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Requests.Logg
{
    public class FilterLogRequest
    {
        [Required(ErrorMessage = "Số lượng bản ghi không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng bản ghi không được nhỏ hơn 1 !")]
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Số trang không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số trang không được nhỏ hơn 1 !")]
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue(null)]
        public DateTime? StartDate { get; set; } = null;
        [DefaultValue(null)]
        public DateTime? EndDate { get; set; } = null;
        [DefaultValue(null)]
        [MaxLength(50, ErrorMessage = "UserName không được quá 50 ký tự !")]
        public string? UserName { get; set; } = null;
        [DefaultValue(null)]
        [MaxLength(50, ErrorMessage = "Url không được quá 100 ký tự !")]
        public string? Url { get; set; } = null;
        [DefaultValue(null)]
        [Range(100, 599, ErrorMessage = "Mã trạng thái phải trong khoảng 100 đến 599.")]
        public int? StatusCode { get; set; } = null;
        [DefaultValue(null)]
        [MaxLength(10, ErrorMessage = "Phương thức HTTP không được quá 10 ký tự")]
        public string? HttpMethod { get; set; } = null;
        [DefaultValue(null)]
        [MaxLength(45, ErrorMessage = "Địa chỉ IP không được quá 45 ký tự")]
        public string? ClientIpAddress { get; set; } = null;
        [DefaultValue(null)]
        [Range(0, int.MaxValue, ErrorMessage = "Thời gian thực thi nhỏ nhất không được nhỏ hơn 0")]
        public int? MinDuration { get; set; } = null;
        [DefaultValue(null)]
        [Range(0, int.MaxValue, ErrorMessage = "Thời gian thực thi lớn nhất không được nhỏ hơn 0")]
        public int? MaxDuration { get; set; } = null;
    }
}
