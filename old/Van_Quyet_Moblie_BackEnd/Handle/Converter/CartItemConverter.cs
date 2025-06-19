using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class CartItemConverter
    {
        private readonly ProductConverter _productConverter;
        public CartItemConverter()
        {
            _productConverter = new ProductConverter();
        }
        public CartItemDTO EntityCartItemToDTO(CartItem cartItem)
        {
            return new CartItemDTO {
                CartItemID = cartItem.ID,
                ProductDTO = _productConverter.EntityProductToDTO(cartItem.Product!),
                Quantity = cartItem.Quantity,
            };
        }

        public List<CartItemDTO> ListCartItemToDTO(List<CartItem> listCartItem) { 
            var listCartItemDTO = new List<CartItemDTO>();
            foreach (var cartItem in listCartItem)
            {
                listCartItemDTO.Add(EntityCartItemToDTO(cartItem));
            }

            return listCartItemDTO;
        }
    }
}
