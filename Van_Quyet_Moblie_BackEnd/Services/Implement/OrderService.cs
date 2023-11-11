using Microsoft.EntityFrameworkCore;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Enums;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.OrderRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly ResponseObject<OrderDTO> _response;
        private readonly TokenHelper _tokenHelper;
        private readonly GHNHelper _ghnHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(ResponseObject<OrderDTO> response, TokenHelper tokenHelper, GHNHelper ghnHelper, IHttpContextAccessor httpContextAccessor)
        {
            _response = response;
            _tokenHelper = tokenHelper;
            _ghnHelper = ghnHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseObject<string>> CancelOrder(int orderID)
        {
            var response = new ResponseObject<string>();
            try
            {
                var order = await _context.Order.Include(x => x.OrderStatus!).FirstOrDefaultAsync(x => x.ID == orderID);
                if (order == null)
                {
                    return response.ResponseError(StatusCodes.Status404NotFound, "Đơn hàng không tồn tại !", null!);
                }
                order.OrderStatus!.ID = (int)StatusOrder.cancel;
                _context.Order.Update(order);
                await _context.SaveChangesAsync();
                return response.ResponseError(StatusCodes.Status200OK, "Hủy đơn hàng thành công !", null!);
            }
            catch (Exception e)
            {
                return response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<ResponseObject<OrderDTO>> CheckOut(CheckOutRequest request)
        {
            try
            {
                InputHelper.CheckOutValidate(request);
                _tokenHelper.IsToken();
                var userID = _tokenHelper.GetUserID();
                if (!await _context.User.AnyAsync(x => x.ID == userID))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Người dùng không tồn tại !", null!);
                }
                if (!await _context.Payment.AnyAsync(x => x.ID == request.PaymentID))
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Phương thức thanh toán không tồn tại !", null!);
                }

                var cart = await _context.Cart.Include(x => x.ListCartItem!).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.UserID == userID);
               
                if (cart == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Không có sản phẩm nào !", null!);
                }

                var listCartItem = cart.ListCartItem;

                using var tran = _context.Database.BeginTransaction();
                try
                {
                    double totalPrice = 0;
                    foreach (var item in listCartItem!)
                    {
                        totalPrice += item.Product!.Price * item.Quantity;
                    }
                    // GHN
                    //string responseGHN = await _ghnHelper.CreateShippingOrderAsync();
                    //VNPAY
                    //var requestPay = new CreateVNPayRequest { 
                    //    OrderType = "electric",
                    //    Name = request.FullName,
                    //    Amount = totalPrice,
                    //    OrderDescription = "Test Thanh Toán"
                    //};

                    //_vnPayHelper.CreatePaymentUrl(requestPay, _httpContextAccessor.HttpContext!);
                    //_vnPayHelper.PaymentExecute(_httpContextAccessor.HttpContext!.Request.Query);
                    //_httpContextAccessor.HttpContext!.Response.Redirect(urlCheckOut);

                    var order = new Order
                    {
                        PaymentID = request.PaymentID,
                        UserID = userID,
                        OriginalPrice = totalPrice,
                        ActualPrice = totalPrice,
                        FullName = request.FullName,
                        Email = request.Email,
                        Phone = request.Phone,
                        Address = request.Address,
                        OrderStatusID = (int)StatusOrder.ready_to_pick,
                    };

                    await _context.Order.AddAsync(order);
                    await _context.SaveChangesAsync();

                    var listOrderDetail = new List<OrderDetail>();
                    foreach (var item in listCartItem!)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderID = order.ID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            PriceTotal = item.Product!.Price * item.Quantity
                        };
                        listOrderDetail.Add(orderDetail);
                    }

                    await _context.OrderDetail.AddRangeAsync(listOrderDetail);
                    await _context.SaveChangesAsync();

                    _context.CartItem.RemoveRange(listCartItem);
                    await _context.SaveChangesAsync();

                    await tran.CommitAsync();

                    var responseOrder = await _context.Order
                        .Include(x => x.Payment!)
                        .Include(x => x.OrderStatus!)
                        .Include(x => x.ListOrderDetail!)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync(x => x.ID == order.ID);
                    return _response.ResponseSuccess("Đặt hàng thành công !", _orderConverter.EntityOrderToDTO(responseOrder!));
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null!);
            }
        }

        public async Task<PageResult<OrderDTO>> GetAllOrder(Pagination pagination)
        {

            var query = _context.Order.Include(x => x.Payment!)
                        .Include(x => x.OrderStatus!)
                        .Include(x => x.ListOrderDetail!)
                        .ThenInclude(x => x.Product).OrderByDescending(x => x.ID).AsQueryable();

            var result = PageResult<Order>.ToPageResult(pagination, query);
            pagination.TotalCount = await query.CountAsync();

            var list = result.ToList();

            return new PageResult<OrderDTO>(pagination, _orderConverter.ListEntityOrderToDTO(result.ToList()));
        }

        public async Task<PageResult<OrderDTO>> GetAllOrderByUserID(Pagination pagination)
        {
            try
            {
                _tokenHelper.IsToken();
                int userID = _tokenHelper.GetUserID();
                var query = _context.Order.Where(x => x.ID == userID).Include(x => x.Payment!)
                            .Include(x => x.OrderStatus!)
                            .Include(x => x.ListOrderDetail!)
                            .ThenInclude(x => x.Product).OrderByDescending(x => x.ID).AsQueryable();

                var result = PageResult<Order>.ToPageResult(pagination, query);
                pagination.TotalCount = await query.CountAsync();

                var list = result.ToList();

                return new PageResult<OrderDTO>(pagination, _orderConverter.ListEntityOrderToDTO(result.ToList()));
            }
            catch (Exception)
            {
                return new PageResult<OrderDTO>(pagination, null!);
            }
        }

        public async Task<ResponseObject<OrderDTO>> GetOrderByID(int orderID)
        {
            try
            {
                var order = await _context.Order
                    .Include(x => x.Payment!)
                    .Include(x => x.OrderStatus!)
                    .Include(x => x.ListOrderDetail!)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.ID == orderID);
                if (order == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Đơn hàng không tồn tại !", null!);
                }

                return _response.ResponseSuccess("Thành công", _orderConverter.EntityOrderToDTO(order));
                
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<ResponseObject<OrderDTO>> UpdateOrder(int orderID, UpdateOrderRequest request)
        {
            try
            {
                InputHelper.UpdateOrderValidate(request);
                var order = await _context.Order.Include(x => x.OrderStatus).FirstOrDefaultAsync(x => x.ID == orderID);
                if (order == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Đơn hàng không tồn tại !", null!);
                }
                if (order!.OrderStatus!.ID == (int)StatusOrder.cancel)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Đơn hàng đã hủy không thể sửa !", null!);
                }
                order.FullName = request.FullName;
                order.Email = request.Email;
                order.Phone = request.Phone;
                order.Address = request.Address;

                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                var responseOrder = await _context.Order
                        .Include(x => x.Payment!)
                        .Include(x => x.OrderStatus!)
                        .Include(x => x.ListOrderDetail!)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync(x => x.ID == order.ID);

                return _response.ResponseSuccess("Cập nhật đơn hàng thành công !", _orderConverter.EntityOrderToDTO(responseOrder!));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }

        public async Task<ResponseObject<OrderDTO>> UpdateOrderStatus(int orderID, int orderStatusID)
        {
            try
            {
                var order = await _context.Order.Include(x => x.OrderStatus).FirstOrDefaultAsync(x => x.ID == orderID);
                if (order == null)
                {
                    return _response.ResponseError(StatusCodes.Status404NotFound, "Đơn hàng không tồn tại !", null!);
                }
                if (order!.OrderStatus!.ID == (int)StatusOrder.cancel)
                {
                    return _response.ResponseError(StatusCodes.Status400BadRequest, "Đơn hàng đã hủy không thể sửa !", null!);
                }
                order.OrderStatus.ID = orderStatusID;

                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                var responseOrder = await _context.Order
                        .Include(x => x.Payment!)
                        .Include(x => x.OrderStatus!)
                        .Include(x => x.ListOrderDetail!)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync(x => x.ID == order.ID);

                return _response.ResponseSuccess("Cập nhật đơn hàng thành công !", _orderConverter.EntityOrderToDTO(responseOrder!));
            }
            catch (Exception e)
            {
                return _response.ResponseError(StatusCodes.Status500InternalServerError, e.Message, null!);
            }
        }
    }
}
