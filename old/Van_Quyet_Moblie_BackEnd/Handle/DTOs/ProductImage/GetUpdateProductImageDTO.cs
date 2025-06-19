namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductImage
{
    public class GetUpdateProductImageDTO
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int ProductID { get; set; }
        public int? ColorID { get; set; }
        public int Status { get; set; }
    }
}
