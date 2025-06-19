namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class ProductImage : BaseEntity
    {
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Product? Product { get; set; }
        public Color? Color { get; set; }
    }
}
