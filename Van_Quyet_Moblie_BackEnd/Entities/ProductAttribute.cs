namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class ProductAttribute : BaseEntity
    {
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Product? Product { get; set; }
        public Color? Color { get; set; }
        public Size? Size { get; set; }
    }
}
