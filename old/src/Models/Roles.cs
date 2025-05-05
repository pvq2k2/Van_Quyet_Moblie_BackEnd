using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Roles : BaseModel
    {
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(30)]
        public string DisplayName { get; set; } = string.Empty;
        [Required, MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        public List<PermissionRole>? ListPermissionRole { get; set; }
        public List<RoleMenu>? ListRoleMenu { get; set; }
    }
}
