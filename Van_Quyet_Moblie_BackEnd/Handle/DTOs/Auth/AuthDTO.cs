namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.Auth
{
    public class AuthDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public int Role { get; set; }

    }
}
