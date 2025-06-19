using Domain.EntityConstants;
using System.ComponentModel.DataAnnotations;

namespace Application.Auth.Dto
{
    public class AuthRegiterDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(UserConsts.MaxUserNameLength, ErrorMessage = "UserName is too long.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(UserConsts.MaxEmailLength, ErrorMessage = "Email is too long.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(UserConsts.MaxPasswordLength, ErrorMessage = "Password is too long.")]
        public required string Password { get; set; }
    }
}
