using Van_Quyet_Moblie_BackEnd.Entities;

namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public double OriginalPrice { get; set; }
        public double ActualPrice { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? OrderCodeGHN { get; set; }

        public PaymentDTO? PaymentDTO { get; set; }
        public OrderStatusDTO? OrderStatusDTO { get; set; }
        public List<OrderDetailDTO>? ListOrderDetailDTO { get; set; }
    }
}
