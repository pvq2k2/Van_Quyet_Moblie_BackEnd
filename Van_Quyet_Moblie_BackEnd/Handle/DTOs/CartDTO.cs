using Van_Quyet_Moblie_BackEnd.Entities;

namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class CartDTO
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public List<CartItemDTO>? ListCartItemDTO { get; set; }
    }
}
