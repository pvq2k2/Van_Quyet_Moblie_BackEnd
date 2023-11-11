using System.ComponentModel.DataAnnotations;

namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class GHNFormat
    {
        /*
        * Tên người gửi.
        * Trường hợp nào nếu không truyền thông tin người gửi thì hệ thống sẽ mặc định lấy thông tin ở ShopID
        */
        [MaxLength(1024)]
        public string? FromName { get; set; }

        // Số điện thoại người gửi.
        [MaxLength(20), Phone]
        public string? FromPhone { get; set; }

        // Địa chỉ người gửi.
        [MaxLength(1024)]
        public string? FromAddress { get; set; }

        // Phường/Xã của người gửi hàng.
        public string? FromWardName { get; set; }

        // Quận/Huyện của người gửi hàng.
        public string? FromDistrictName { get; set; }

        // Tỉnh của người gửi hàng.
        public string? FromProvinceName { get; set; }

        // Tên người nhận hàng.
        [MaxLength(100), Required]
        public string? ToName { get; set; } // Require

        // Số điện thoại người nhận hàng.
        [MaxLength(20), Phone, Required]
        public string? ToPhone { get; set; } // Require

        // Địa chỉ Shiper tới giao hàng.
        [MaxLength(1024), Required]
        public string? ToAddress { get; set; } // Require

        // Phường/Xã của người nhận hàng.
        [Required]
        public string? ToWardName { get; set; } // Require

        // Quận/Huyện của người nhận hàng.
        [Required]
        public string? ToDistrictName { get; set; } // Require

        // Tỉnh của người nhận hàng.
        [Required]
        public string? ToProvinceName { get; set; } // Require

        // Số điện thoại trả hàng khi không giao được.
        public string? ReturnPhone { get; set; }

        // Địa chỉ trả hàng khi không giao được.
        public string? ReturnAddress { get; set; }

        // Quận/Huyện của người nhận hàng trả.
        public string? ReturnDistrictName { get; set; }

        // Phường/Xã của người nhận hàng trả.
        public string? ReturnWardName { get; set; }

        /*
         * Mã đơn hàng riêng của khách hàng.
         * Giá trị mặc định: null
         * Lưu ý : Client_order_code thì sẽ lấy lại đơn hàng đã có Client_order_code này
        */
        public string? ClientOrderCode { get; set; }

        /*
         * Tiền thu hộ cho người gửi.
         * Maximum :10.000.000
         * Giá trị mặc định: 0
         */
        [Range(0, 10000000)]
        public int CodAmount { get; set; } = 0;

        // Nội dung của đơn hàng.
        [MaxLength(2000)]
        public string? Content { get; set; }

        /*
         * Khối lượng của đơn hàng (gram).
         * Tối đa : 30000 gram
         */
        [Range(1, 30000), Required]
        public int Weight { get; set; } // Require

        /*
         * Chiều dài của đơn hàng (cm).
         * Tối đa : 150 cm
         */
        [Range(1, 150), Required]
        public int Length { get; set; } // Require

        /*
        * Chiều rộng của đơn hàng (cm).
        * Tối đa : 150 cm
        */
        [Range(1, 150), Required]
        public int Width { get; set; } // Require

        /*
        * Chiều cao của đơn hàng (cm).
        * Tối đa : 150 cm
        */
        [Range(1, 150), Required]
        public int Height { get; set; } // Require

        /*
         * Mã bưu cục để gửi hàng tại điểm.
         * Giá trị mặc định : null
         * Giá trị truyền vào > 0
        */
        public int? PickStationId { get; set; }

        /*
         * Giá trị của đơn hàng ( Trường hợp mất hàng , bể hàng sẽ đền theo giá trị của đơn hàng).
         * Tối đa 5.000.000
         * Giá trị mặc định: 0
         */
        [Range(0, 5000000)]
        public int InsuranceValue { get; set; }

        // Mã giảm giá.
        public string? Coupon { get; set; }

        /*
         * Mã loại dịch vụ: Gọi API lấy gói dịch vụ để lấy mã loại dịch vụ.
         * Mã loại dịch vụ cố định. Trong đó:  2: Chuyển phát thương mại điện tử, 5: Chuyển phát truyền thống
         * Lưu ý: Nếu đã truyền service_id thì không cần truyền service_type_id.
         */
        [Required]
        public int ServiceTypeId { get; set; } // Require

        /*
         * Để lấy thông tin chính xác từng dịch vụ phù hợp với tuyến đường nên gọi API lấy gói dịch vụ
         * Lưu ý: Nếu đã truyền service_type_id thì không cần truyền service_id.
         */
        [Required]
        public int ServiceId { get; set; } = 0; // Require

        /*
         * Mã người thanh toán phí dịch vụ.
         * 1: Người bán/Người gửi.
         * 2: Người mua/Người nhận.
         */
        [Required]
        public int PaymentTypeId { get; set; } // Require

        // Người gửi ghi chú cho tài xế.
        [MaxLength(5000)]
        public string? Note { get; set; }

        /*
         * Ghi chú bắt buộc, Bao gồm: CHOTHUHANG, CHOXEMHANGKHONGTHU, KHONGCHOXEMHANG
         * CHOTHUHANG nghĩa là Người mua có thể yêu cầu xem và dùng thử hàng hóa
         * CHOXEMHANGKHONGTHU nghĩa là Người mua được xem hàng nhưng không được dùng thử hàng
         * KHONGCHOXEMHANG nghĩa là Người mua không được phép xem hàng
         */
        [Required]
        public string? RequiredNote { get; set; } // Require

        // Dùng để truyền ca lấy hàng , Sử dụng API Lấy danh sách ca lấy
        public List<int>? PickShift { get; set; }

        // Truyền thời gian mong muốn lấy hàng , định dạng UnixtimeStamp
        public int PickupTime { get; set; }

        /*
         * Thông tin sản phẩm.
         * Bắt buộc truyền Item khi sử dụng gói dịch vụ Chuyển phát truyền thống
         */
        //[Required]
        //public List<Item>? Items { get; set; } // Require
    }
    //public class Item
    //{
    //    // 	Tên của sản phẩm.
    //    [Required]
    //    public string? Name { get; set; } // Require

    //    // Mã của sản phẩm.
    //    public string? Code { get; set; }

    //    // Số lượng của sản phẩm.
    //    [Required]
    //    public int Quantity { get; set; } // Require

    //    // Giá của sản phẩm.
    //    public int Price { get; set; }

    //    // Chiều dài của sản phẩm.chuyển phát truyền thống đi nhiều kiện thì bắt buộc phải truyền length
    //    public int Length { get; set; }

    //    // Chiều rộng của sản phẩm.chuyển phát truyền thống đi nhiều kiện thì bắt buộc phải truyền width
    //    public int Width { get; set; }

    //    // Đối với trường hợp chọn gói dịch chuyển phát truyền thống đi nhiều kiện thì bắt buộc phải truyền weight
    //    public int Weight { get; set; }

    //    // Chiều cao của sản phẩm.chuyển phát truyền thống đi nhiều kiện thì bắt buộc phải truyền height
    //    public int Height { get; set; }
    //    //public Category Category { get; set; }

    //    // Thu thêm tiền khi giao hàng thất bại
    //    public int CodFailedAmount { get; set; }
    //}

    //public class Category
    //{
    //    public string Level1 { get; set; }
    //    public string Level2 { get; set; }
    //    public string Level3 { get; set; }
    //}
}
