namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Size : BaseEntity
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
