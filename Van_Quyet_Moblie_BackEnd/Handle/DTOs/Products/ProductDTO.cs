namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public int? Discount { get; set; }
        public int Status { get; set; }
        public int NumberOfViews { get; set; } = 0;
    }
}
