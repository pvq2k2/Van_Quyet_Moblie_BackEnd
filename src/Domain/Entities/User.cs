using System.ComponentModel.DataAnnotations;
using Domain.EntityConstants;

namespace Domain.Entities
{
    public class User: BaseEntity
    {
        [Required, MaxLength(UserConsts.MaxUserNameLength)]
        public required string UserName { get; set; }
        [Required, MaxLength(UserConsts.MaxEmailLength)]
        public required string Email { get; set; }
        [Required, MaxLength(UserConsts.MaxPasswordLength)]
        public required string Password { get; set; }
        public string? OtpCode { get; set; }
        public int OtpFailedCount { get; set; } = 0;
        public DateTime? OtpCodeExpiry { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? ResetPasswordToken { set; get; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public int Status { set; get; } = 1;

        [Required]
        public int RoleId { get; set; } = 1; // Default role is User


        // Navigation property - Một User có nhiều DeliveryInfo
        public ICollection<DeliveryInfo>? ListDeliveryInfo { get; set; }
        public Role? Role { get; set; }
    }
}
