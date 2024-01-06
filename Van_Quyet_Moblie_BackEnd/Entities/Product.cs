namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Product : BaseEntity
    {
        public int SubCategoriesID { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public int? Discount { get; set; }
        public int Status { get; set; }
        public int NumberOfViews { get; set; } = 0;
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<ProductAttribute>? ListProductAttribute { get; set; }
        public SubCategories? SubCategories { get; set; }
        public List<ProductImage>? ListProductImage { get; set; }
        public List<ProductReview>? ListProductReview { get; set; }
        public List<CartItem>? ListCartItem { get; set; }
        public List<OrderDetail>? ListOrderDetail { get; set; }
    }
}
