using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Permissions: BaseModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;
        [Required, MaxLength(300)]
        public string Description { get; set; } = string.Empty;
    }
}
