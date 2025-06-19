using System.ComponentModel.DataAnnotations;

namespace Application.Auth.Dto
{
    public class AuthChangePasswordDto
    {
        [Required(ErrorMessage = "OldPassword is required.")]
        public string? OldPassword { get; set; }
        [Required(ErrorMessage = "NewPassword is required.")]
        public string? NewPassword { get; set; }
    }
}
