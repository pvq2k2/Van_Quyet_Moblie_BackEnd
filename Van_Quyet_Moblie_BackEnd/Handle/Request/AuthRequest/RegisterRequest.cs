namespace Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile? Avatar { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
