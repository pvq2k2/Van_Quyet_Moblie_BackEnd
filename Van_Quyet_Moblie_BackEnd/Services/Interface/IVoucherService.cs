using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Request.VoucherRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Response;

namespace Van_Quyet_Moblie_BackEnd.Services.Interface
{
    public interface IVoucherService
    {
        public Task<ResponseObject<VoucherDTO>> CreateVoucher(VoucherRequest request);
        public Task<PageResult<VoucherDTO>> GetAllVoucher(Pagination pagination);
        public Task<ResponseObject<VoucherDTO>> GetVoucherByID(int voucherID);
        public Task<ResponseObject<VoucherDTO>> UpdateVoucher(int voucherID, VoucherRequest request);
        public Task<ResponseObject<string>> RemoveVoucer(int voucherID);
        public Task<ResponseObject<VoucherDTO>> CheckVoucer(string code, int amount);
        public Task<ResponseObject<string>> RemoveVoucerByUser(int voucherID);

    }
}
