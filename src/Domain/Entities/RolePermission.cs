using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int PermissionId { get; set; }
        public Role? Role { get; set; }
        public Permission? Permission { get; set; }
    }
}
