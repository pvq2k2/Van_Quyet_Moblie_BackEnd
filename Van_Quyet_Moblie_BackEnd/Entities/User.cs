namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class User : BaseEntity
    {
        public string? FullName { get; set; }
        public string? NumberPhone { get; set; }
        public string? Avatar { get; set; }
        public int Gender { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int AccountID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Account? Account { get; set; }
        public List<Order>? ListOrder { get; set; }
        public List<ProductReview>? ListProductReview { get; set; }
        public List<Cart>? ListCart { get; set; }
        public List<Voucher>? Voucher { get; set; }
    }
}
