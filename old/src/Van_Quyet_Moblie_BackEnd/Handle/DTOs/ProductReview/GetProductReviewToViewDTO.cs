namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductReview
{
    public class GetProductReviewToViewDTO
    {
        public int ID { get; set; }
        public string? ContentRated { get; set; }
        public int PointEvaluation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
    }
}
