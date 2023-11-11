using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CartRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class CartService : BaseService, ICartService
    {
        private readonly ResponseObject<CartDTO> _response;
        private readonly TokenHelper _tokenHelper;

        public CartService(ResponseObject<CartDTO> response, TokenHelper tokenHelper)
        {
            _response = response;
            _tokenHelper = tokenHelper;
        }
        public async Task<ResponseObject<CartDTO>> AddToCart(AddToCartRequest request)
        {
            try
            {
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();

                if (!await _context.User.AnyAsync(x => x.ID == userID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại !", null!);
                }
                if (!await _context.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }
                var cart = await _context.Cart.FirstOrDefaultAsync(x => x.ID == userID);
                using var tran = _context.Database.BeginTransaction();
                try
                {
                    if (cart == null)
                    {
                        cart = new Cart
                        {
                            UserID = userID,
                        };
                        await _context.Cart.AddAsync(cart);
                        await _context.SaveChangesAsync();
                    }

                    var cartItem = await _context.CartItem.FirstOrDefaultAsync(x => x.CartID == cart.ID && x.ProductID == request.ProductID);
                    if (cartItem == null)
                    {
                        cartItem = new CartItem
                        {
                            CartID = cart.ID,
                            ProductID = request.ProductID,
                            Quantity = request.Quantity
                        };
                        await _context.CartItem.AddAsync(cartItem);
                        await _context.SaveChangesAsync();
                    } else
                    {
                        cartItem.Quantity += request.Quantity;
                    }

                    _context.CartItem.Update(cartItem);
                    await _context.SaveChangesAsync();

                    await tran.CommitAsync();
                    var responseCart = await _context.Cart.Include(x => x.ListCartItem!).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.ID == cart.ID);
                    return _response.ResponseSuccess("Thêm vào giỏ hàng thành công !", _cartConverter.EntityCartToDTO(responseCart!));
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
                }
            }
            catch (Exception ex)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }

        }

        public async Task<ResponseObject<CartDTO>> GetAllCartItemWhereCartExist()
        {
            try
            {
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();

                if (!await _context.User.AnyAsync(x => x.ID == userID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại !", null!);
                }
                var cart = await _context.Cart.Include(x => x.ListCartItem!).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.UserID == userID);
                if (cart == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Không có sản phẩm nào !", null!);
                }

                return _response.ResponseSuccess("Thành công !", _cartConverter.EntityCartToDTO(cart!));
            }
            catch (Exception ex)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }

        }
        public async Task<ResponseObject<CartDTO>> UpdateQuantityCartItemExistInCart(UpdateQuantityCartItemRequest request)
        {
            try
            {
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();

                if (request.Quantity < 1)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Số lượng không được nhỏ hơn 1 !", null!);
                }
                if (!await _context.User.AnyAsync(x => x.ID == userID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại !", null!);
                }
                if (!await _context.Product.AnyAsync(x => x.ID == request.ProductID))
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại !", null!);
                }
                var cart = await _context.Cart.FirstOrDefaultAsync(x => x.ID == userID);
                if (cart == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Giỏ hàng không tồn tại !", null!);
                }
                var cartItem = await _context.CartItem.FirstOrDefaultAsync(x => x.CartID == cart.ID && x.ProductID == request.ProductID);
                if (cartItem == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm không tồn tại trong giỏ hàng !", null!);
                }

                cart.UpdatedAt = DateTime.Now;
                cartItem.Quantity = request.Quantity;
                cartItem.UpdatedAt = DateTime.Now;
                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();
                _context.CartItem.Update(cartItem);
                await _context.SaveChangesAsync();

                var responseCart = await _context.Cart.Include(x => x.ListCartItem!).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.ID == cart.ID);
                return _response.ResponseSuccess("Thành công !", _cartConverter.EntityCartToDTO(responseCart!));
            }
            catch (Exception ex)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }

        public async Task<ResponseObject<string>> RemoveCartItem(int cartItemID)
        {
            var response = new ResponseObject<string>();
            try
            {
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();
                if (!await _context.User.AnyAsync(x => x.ID == userID))
                {
                    return response.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại !", null!);
                }
                var cartItem = await _context.CartItem.FirstOrDefaultAsync(x => x.ID == cartItemID);
                if (cartItem == null)
                {
                    return response.ResponseError(StatusCodes.Status404NotFound, "Sản phẩm trong giỏ hàng không tồn tại !", null!);
                }
                _context.CartItem.Remove(cartItem);
                await _context.SaveChangesAsync();

                return response.ResponseSuccess("Xóa thành công !", null!);
            }
            catch (Exception ex)
            {
                return response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }


    }
}
