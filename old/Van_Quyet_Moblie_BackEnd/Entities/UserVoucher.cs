namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class UserVoucher : BaseEntity
    {
        public int UserID { get; set; }
        public int VoucherID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public Voucher? Voucher { get; set; }
    }
}
