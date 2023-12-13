namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class ProductType : BaseEntity
    {
        public int ProductID { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Product? Product { get; set; }
    }
}
