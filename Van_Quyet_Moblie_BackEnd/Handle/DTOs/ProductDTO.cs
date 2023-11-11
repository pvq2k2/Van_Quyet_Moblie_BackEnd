namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string? NameProduct { get; set; }
        public double Price { get; set; }
        public string? AvatarImageProduct { get; set; }
        public string? Title { get; set; }
        public int? Discount { get; set; }
        public int Status { get; set; }
        public int NumberOfViews { get; set; } = 0;
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
    }
}
