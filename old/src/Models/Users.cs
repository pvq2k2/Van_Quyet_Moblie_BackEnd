using Enums;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Users : BaseModel
    {
        [MaxLength(30)]
        public string? UserName { get; set; }
        [MaxLength(50)]
        public string? FullName { get; set; }
        [MaxLength(100), Required]
        public string? Email { get; set; }
        [MaxLength(100), Required]
        public string? Password { get; set; }
        [MaxLength(15)]
        public string? NumberPhone { get; set; }
        [MaxLength(255)]
        public string? Avatar { get; set; }
        [Required]
        public int Gender { get; set; }
        public int Status { get; set; }

        public List<RoleUser>? ListRoleUser { get; set; }
    }
}
