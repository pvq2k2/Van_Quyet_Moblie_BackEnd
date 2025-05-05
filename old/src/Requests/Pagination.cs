using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Requests
{
    public class Pagination
    {
        [Required(ErrorMessage = "Số lượng bản ghi không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng bản ghi không được nhỏ hơn 1 !")]
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Số trang không được trống !")]
        [Range(1, int.MaxValue, ErrorMessage = "Số trang không được nhỏ hơn 1 !")]
        [DefaultValue(1)]
        public int PageNumber { get; set; }

        public int TotalCount { get; set; }

        public int TotalPage
        {
            get
            {
                if (TotalCount == 0) return 0;
                var total = (int)Math.Ceiling((double)TotalCount / PageSize);
                return total;
            }
        }

        public Pagination()
        {
            PageSize = 10;
            PageNumber = 1;
        }
    }
}
