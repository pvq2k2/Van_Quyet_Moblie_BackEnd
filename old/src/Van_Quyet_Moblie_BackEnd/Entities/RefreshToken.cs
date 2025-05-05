namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string? Token { get; set; }
        public int AccountID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiredTime { get; set; }
        public Account? Account { get; set; }
    }
}
