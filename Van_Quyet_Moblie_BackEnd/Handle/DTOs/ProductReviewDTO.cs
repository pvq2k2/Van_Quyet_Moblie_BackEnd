namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class ProductReviewDTO
    {
        public int ProductReviewID { get; set; }
        public string? ContentRated { get; set; }
        public int PointEvaluation { get; set; }
        public string? ContentSeen { get; set; }
        public int Status { get; set; }
    }
}
