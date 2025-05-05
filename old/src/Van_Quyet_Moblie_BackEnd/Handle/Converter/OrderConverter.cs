using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class OrderConverter
    {
        private readonly OrderDetailConverter _orderDetailConverter;
        private readonly OrderStatusConverter _orderStatusConverter;
        private readonly PaymentConverter _paymentConverter;
        public OrderConverter()
        {
            _orderDetailConverter = new OrderDetailConverter();
            _orderStatusConverter = new OrderStatusConverter();
            _paymentConverter = new PaymentConverter();
        }

        public OrderDTO EntityOrderToDTO(Order order)
        {
            return new OrderDTO
            {
                OrderID = order.ID,
                OriginalPrice = order.OriginalPrice,
                ActualPrice = order.ActualPrice,
                FullName = order.FullName,
                Email = order.Email,
                Phone = order.Phone,
                Address = order.Address,
                OrderCodeGHN = order.OrderCodeGHN,

                PaymentDTO = _paymentConverter.EntityPaymentToDTO(order.Payment!),
                OrderStatusDTO = _orderStatusConverter.EntityOrderStatusToDTO(order.OrderStatus!),
                ListOrderDetailDTO = _orderDetailConverter.ListEntityOrderDetailToDTO(order.ListOrderDetail!)
            };
        }
        public List<OrderDTO> ListEntityOrderToDTO(List<Order> listOrder)
        {
            var listOrderDTO = new List<OrderDTO>();
            foreach (var item in listOrder)
            {
                listOrderDTO.Add(EntityOrderToDTO(item));
            }

            return listOrderDTO;
        }
    }
}
