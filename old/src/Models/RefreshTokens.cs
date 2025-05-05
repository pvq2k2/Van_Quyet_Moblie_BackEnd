using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class RefreshTokens: BaseModel
    {
        [Required, MaxLength(256)]
        public string Token { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiredTime { get; set; }
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users? Users { get; set; }
    }
}
