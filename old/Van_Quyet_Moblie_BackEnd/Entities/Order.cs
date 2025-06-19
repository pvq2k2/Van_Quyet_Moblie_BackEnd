namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Order : BaseEntity
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public double OriginalPrice { get; set; }
        public double ActualPrice { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int OrderStatusID { get; set; }

        public int Fee { get; set; } // Phí
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? OrderCodeGHN { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Payment? Payment { get; set; }
        public User? User { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public List<OrderDetail>? ListOrderDetail { get; set; }
    }
}
