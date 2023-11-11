namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class ProductType : BaseEntity
    {
        public string? NameProductType { get; set; }
        public string? ImageTypeProduct { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<Product>? ListProduct { get; set; }
    }
}
