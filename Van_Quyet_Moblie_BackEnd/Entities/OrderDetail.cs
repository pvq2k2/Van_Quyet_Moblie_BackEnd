namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public double PriceTotal { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
