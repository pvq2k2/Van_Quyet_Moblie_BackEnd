using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CartRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface ICartService
    {
        public Task<ResponseObject<CartDTO>> AddToCart(AddToCartRequest request);
        public Task<ResponseObject<CartDTO>> GetAllCartItemWhereCartExist();
        public Task<ResponseObject<CartDTO>> UpdateQuantityCartItemExistInCart(UpdateQuantityCartItemRequest request);
        public Task<ResponseObject<string>> RemoveCartItem(int cartItemID);

    }
}
