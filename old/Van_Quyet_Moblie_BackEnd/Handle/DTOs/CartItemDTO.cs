using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products;

namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class CartItemDTO
    {
        public int CartItemID { get; set; }
        public ProductDTO? ProductDTO { get; set; }
        public int Quantity { get; set; }
    }
}
