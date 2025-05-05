namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest
{
    public class UpdateProductReviewRequest
    {
        public int ProductID { get; set; }
        public string? ContentRated { get; set; }
        public int PointEvaluation { get; set; }
        public int Status { get; set; }
    }
}
