using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RoleUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }
        [ForeignKey("RoleId")]
        public Roles? Role { get; set; }
    }
}
