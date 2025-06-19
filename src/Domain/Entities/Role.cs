using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public ICollection<RolePermission> ListRolePermission { get; set; } = new List<RolePermission>();
    }
}
