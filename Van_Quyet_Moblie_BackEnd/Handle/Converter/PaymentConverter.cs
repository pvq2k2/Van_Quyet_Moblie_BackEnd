using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class PaymentConverter
    {
        public PaymentDTO EntityPaymentToDTO(Payment payment)
        {
            return new PaymentDTO
            {
                PaymentID = payment.ID,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status
            };
        }
    }
}
