namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Payment : BaseEntity
    {
        public string? PaymentMethod { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<Order>? ListOrder { get; set; }
    }
}
