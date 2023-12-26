namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class SubCategories : BaseEntity
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Slug { get; set; }
        public int CategoriesID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        public Categories? Categories { get; set; }
        public List<Product>? ListProduct { get; set; }

    }
}
