using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.OrderRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _iOrderService;
        public OrderController(IOrderService iOrderService)
        {
            _iOrderService = iOrderService;
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut(CheckOutRequest request)
        {
            return Ok(await  _iOrderService.CheckOut(request));
        }

        [HttpPost("GetAllOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrder(Pagination pagination)
        {
            return Ok(await _iOrderService.GetAllOrder(pagination));
        }

        [HttpPost("GetAllOrderByUserID")]
        public async Task<IActionResult> GetAllOrderByUserID(Pagination pagination)
        {
            return Ok(await _iOrderService.GetAllOrderByUserID(pagination));
        }

        [HttpGet("GetOrderByID")]
        public async Task<IActionResult> GetOrderByID(int orderID)
        {
            return Ok(await _iOrderService.GetOrderByID(orderID));
        }

        [HttpPatch("UpdateOrderStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderID, int orderStatusID)
        {
            return Ok(await _iOrderService.UpdateOrderStatus(orderID, orderStatusID));
        }

        [HttpPatch("UpdateOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int orderID, UpdateOrderRequest request)
        {
            return Ok(await _iOrderService.UpdateOrder(orderID, request));
        }

        [HttpPatch("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderID)
        {
            return Ok(await _iOrderService.CancelOrder(orderID));
        }
    }
}
