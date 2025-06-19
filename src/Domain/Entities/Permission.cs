using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Permission : BaseEntity
    {
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        // Navigation property - Một Permission có nhiều RolePermission
        public ICollection<RolePermission> ListRolePermission { get; set; } = new List<RolePermission>();
    }
}
