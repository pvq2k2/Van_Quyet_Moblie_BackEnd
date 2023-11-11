using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.VoucherRequest;
using Van_Quyet_Moblie_BackEnd.Services.Implement;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _iVoucherService;

        public VoucherController(IVoucherService iVoucherService)
        {
            _iVoucherService = iVoucherService;
        }

        [HttpPost("GetAllVoucher")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVoucher(Pagination pagination)
        {
            return Ok(await _iVoucherService.GetAllVoucher(pagination));
        }

        [HttpGet("GetVoucherByID")]
        public async Task<IActionResult> GetVoucherByID(int voucherID)
        {
            return Ok(await _iVoucherService.GetVoucherByID(voucherID));
        }

        [HttpPost("CreateVoucher")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVoucher(VoucherRequest request)
        {
            return Ok(await _iVoucherService.CreateVoucher(request));
        }

        [HttpPatch("UpdateVoucher")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVoucher(int voucherID, VoucherRequest request)
        {
            return Ok(await _iVoucherService.UpdateVoucher(voucherID, request));
        }

        [HttpDelete("RemoveVoucer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveVoucer(int voucherID)
        {
            return Ok(await _iVoucherService.RemoveVoucer(voucherID));
        }

        [HttpPatch("CheckVoucer")]
        public async Task<IActionResult> CheckVoucer(string code, int amount)
        {
            return Ok(await _iVoucherService.CheckVoucer(code, amount));
        }

        [HttpPatch("RemoveVoucerByUser")]
        public async Task<IActionResult> RemoveVoucerByUser(int voucherID)
        {
            return Ok(await _iVoucherService.RemoveVoucerByUser(voucherID));
        }
    }
}
