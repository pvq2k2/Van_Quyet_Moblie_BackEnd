namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest
{
    public class CreateProductImageRequest
    {
        public string? Title { get; set; }
        public IFormFile? Image { get; set; }
        public int ColorID { get; set; } = 0;
        public int ProductID { get; set; }
    }
}
