namespace Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest
{
    public class ChangeInformationRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string NumberPhone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Gender { get; set; }
        public IFormFile? Avatar { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
