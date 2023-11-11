using System.ComponentModel.DataAnnotations;

namespace Van_Quyet_Moblie_BackEnd.Handle.Request.VNPayRequest
{
    public class GetBillRequest
    {
        [MinLength(1)]
        [MaxLength(32)]
        [Required]
        public string? vnp_RequestId { get; set; }
        [MinLength(1)]
        [MaxLength(8)]
        [Required]
        public string vnp_Version { get; set; } = "2.1.0";
        [MinLength(1)]
        [MaxLength(16)]
        [Required]
        public string vnp_Command { get; set; } = "querydr";
        [MinLength(1)]
        [MaxLength(8)]
        [Required]
        public string? vnp_TmnCode { get; set; }
        [MinLength(1)]
        [MaxLength(100)]
        [Required]
        public string? vnp_TxnRef { get; set; }
        [MinLength(1)]
        [MaxLength(255)]
        [Required]
        public string? vnp_OrderInfo { get; set; }
        [Range(1, 15)]
        [Required]
        public int? vnp_TransactionNo { get; set; }
        [Range(1, 14)]
        [Required]
        public int? vnp_TransactionDate { get; set; }
        [Range(1, 14)]
        [Required]
        public int? vnp_CreateDate { get; set; }
        [MinLength(7)]
        [MaxLength(45)]
        [Required]
        public string? vnp_IpAddr { get; set; }
        [MinLength(32)]
        [MaxLength(256)]
        [Required]
        public string? vnp_SecureHash { get; set; }
    }
}
