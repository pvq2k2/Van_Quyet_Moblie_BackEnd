namespace Van_Quyet_Moblie_BackEnd.Handle.Request.VNPayRequest
{
    public class PaymentResultRequest
    {
        public string? Amount { get; set; }
        public string? BankCode { get; set; }
        public string? BankTranNo { get; set; }
        public string? CardType { set; get; }
        public string? OrderInfo { get; set; }
        public string? PayDate { get; set; }
        public string? ResponseCode { get; set; }
        public string? SecureHash { get; set; }
        public string? TmnCode { get; set; }
        public string? TransactionNo { get; set; }
        public string? TransactionStatus { get; set; }
        public string? TxnRef { get; set; }
    }
}
