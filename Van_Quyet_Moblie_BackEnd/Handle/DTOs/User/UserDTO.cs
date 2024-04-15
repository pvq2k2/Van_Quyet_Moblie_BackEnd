namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.User
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string? UserName { get; set; }
        public int Status { set; get; } = 0;
        public int DecentralizationID { set; get; }
        public string? FullName { get; set; }
        public string? NumberPhone { get; set; }
        public string? Avatar { get; set; }
        public int Gender { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
