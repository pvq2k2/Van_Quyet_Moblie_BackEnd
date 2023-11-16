namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class ProductType : BaseEntity
    {
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<Product>? ListProduct { get; set; }
    }
}
