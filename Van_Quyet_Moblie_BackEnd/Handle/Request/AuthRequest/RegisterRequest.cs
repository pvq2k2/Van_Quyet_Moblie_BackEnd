namespace Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? NumberPhone { get; set; }
        public int Gender { get; set; }
        public string? Email { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
