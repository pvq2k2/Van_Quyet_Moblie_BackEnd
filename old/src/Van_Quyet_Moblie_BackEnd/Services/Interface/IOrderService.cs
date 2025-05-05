using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.OrderRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IOrderService
    {
        public Task<ResponseObject<OrderDTO>> CheckOut(CheckOutRequest request);
        public Task<PageResult<OrderDTO>> GetAllOrder(Pagination pagination);
        public Task<PageResult<OrderDTO>> GetAllOrderByUserID(Pagination pagination);
        public Task<ResponseObject<OrderDTO>> GetOrderByID(int orderID);
        public Task<ResponseObject<OrderDTO>> UpdateOrderStatus(int orderID, int orderStatusID);
        public Task<ResponseObject<OrderDTO>> UpdateOrder(int orderID, UpdateOrderRequest request);
        public Task<ResponseObject<string>> CancelOrder(int orderID);
    }
}
