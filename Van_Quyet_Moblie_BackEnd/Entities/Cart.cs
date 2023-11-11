namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Cart : BaseEntity
    {
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public User? User { get; set; }
        public List<CartItem>? ListCartItem { get; set; }
    }
}
