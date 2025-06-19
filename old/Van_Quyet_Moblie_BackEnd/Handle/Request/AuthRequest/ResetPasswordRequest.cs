namespace Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest
{
    public class ResetPasswordRequest
    {
        public string? Token { get; set; }
        public string? Password { get; set; }
    }
}
