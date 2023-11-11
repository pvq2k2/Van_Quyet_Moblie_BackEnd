namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class VoucherDTO
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public int DiscountPercentage { get; set; } // Phần trăm giảm giá áp dụng cho đơn hàng.
        public double MinimumPurchaseAmount { get; set; } // Số tiền tối thiểu mà khách hàng cần chi trước khi có thể sử dụng mã giảm giá
        public DateTime ExpiryDate { get; set; }
    }
}
