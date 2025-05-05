namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class ProductReview : BaseEntity
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string? ContentRated { get; set; }
        public int PointEvaluation { get; set; }
        public string? ContentSeen { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
