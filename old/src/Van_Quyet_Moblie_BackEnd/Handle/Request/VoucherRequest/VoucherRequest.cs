namespace Van_Quyet_Moblie_BackEnd.Handle.Request.VoucherRequest
{
    public class VoucherRequest
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public int DiscountPercentage { get; set; }
        public double MinimumPurchaseAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
