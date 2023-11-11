using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class OrderStatusConverter
    {
        public OrderStatusDTO EntityOrderStatusToDTO(OrderStatus orderStatus)
        {
            return new OrderStatusDTO
            {
                OrderStatusID = orderStatus.ID,
                StatusName = orderStatus.StatusName
            };
        }
    }
}
