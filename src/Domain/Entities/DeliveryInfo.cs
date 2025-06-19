using System.ComponentModel.DataAnnotations;
using Domain.EntityConstants;

namespace Domain.Entities
{
    public class DeliveryInfo: BaseEntity
    {
        [Required]
        [MaxLength(DeliveryInfoConsts.MaxReceiverNameLength)]
        public required string ReceiverName { get; set; }
        [Required]
        [MaxLength(DeliveryInfoConsts.MaxAddressLength)]
        public required string Address { get; set; }
        [Required]
        [MaxLength(DeliveryInfoConsts.MaxPhoneNumberLength)]
        public required string PhoneNumber { get; set; }


        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
