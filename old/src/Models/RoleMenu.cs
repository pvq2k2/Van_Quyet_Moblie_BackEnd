using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RoleMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MenuId { get; set; }
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("MenuId")]
        public Menus? Menu { get; set; }

        [ForeignKey("RoleId")]
        public Roles? Role { get; set; }
    }
}
