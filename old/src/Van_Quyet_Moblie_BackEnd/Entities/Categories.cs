namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Categories : BaseEntity
    {
        public char Icon { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<SubCategories>? ListSubCategories { get; set; }
    }
}
