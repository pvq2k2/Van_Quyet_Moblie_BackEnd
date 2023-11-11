namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest
{
    public class UpdateProductRequest
    {
        public string? NameProduct { get; set; }
        public int ProductTypeID { get; set; }
        public double Price { get; set; }
        public IFormFile? AvatarImageProduct { get; set; }
        public string? Title { get; set; }
        public int? Discount { get; set; }
        public int Status { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
    }
}
