using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PermissionRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PermissionId { get; set; }
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("PermissionId")]
        public Permissions? Permission { get; set; }

        [ForeignKey("RoleId")]
        public Roles? Role { get; set; }
    }
}
