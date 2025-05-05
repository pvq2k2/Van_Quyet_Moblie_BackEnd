namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class CartItem : BaseEntity
    {
        public int ProductID { get; set; }
        public int CartID { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Product? Product { get; set; }
        public Cart? Cart { get; set; }
    }
}
