namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Account : BaseEntity
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Status { set; get; } = 0;
        public int DecentralizationID { set; get; }
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? ResetPasswordToken { set; get; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Decentralization? Decentralization { set; get; }
        public RefreshToken? RefreshToken { set; get; }
        public User? User { get; set; }
    }
}
