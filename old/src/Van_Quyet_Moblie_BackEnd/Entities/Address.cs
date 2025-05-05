namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Address : BaseEntity
    {
        public string? DetailAddress { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public User? User { get; set; }
    }
}
