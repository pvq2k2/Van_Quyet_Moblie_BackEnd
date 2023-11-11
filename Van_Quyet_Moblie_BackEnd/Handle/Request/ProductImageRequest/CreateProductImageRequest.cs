namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest
{
    public class CreateProductImageRequest
    {
        public string? Title { get; set; }
        public IFormFile? ImageProduct { get; set; }
        public int ProductID { get; set; }
    }
}
