using System.ComponentModel.DataAnnotations;

namespace Application.Auth.Dto
{
    public class AuthLoginDto
    {
        [Required(ErrorMessage = "Username or email is required.")]
        public required string UsernameOrEmail { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}
