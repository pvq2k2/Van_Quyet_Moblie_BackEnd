using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest;
using Van_Quyet_Moblie_BackEnd.Services.Implement;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _iProductReviewService;

        public ProductReviewController(IProductReviewService iProductReviewService)
        {
            _iProductReviewService = iProductReviewService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllProductReview")]
        public async Task<IActionResult> GetAllProductReview(Pagination pagination)
        {
            return Ok(await _iProductReviewService.GetAllProductReview(pagination));
        }

        [HttpGet("GetProductReviewByID")]
        public async Task<IActionResult> GetProductByID(int productID)
        {
            return Ok(await _iProductReviewService.GetProductReviewByID(productID));
        }

        [Authorize]
        [HttpPost("CreateProductReview")]
        public async Task<IActionResult> CreateProductReview(CreateProductReviewRequest request)
        {
            return Ok(await _iProductReviewService.CreateProductReview(request));
        }

        [Authorize]
        [HttpPut("UpdateProductReview")]
        public async Task<IActionResult> UpdateProductReview(int productReviewID, UpdateProductReviewRequest request)
        {
            return Ok(await _iProductReviewService.UpdateProductReview(productReviewID, request));
        }

        [Authorize]
        [HttpDelete("RemoveProductReview")]
        public async Task<IActionResult> RemoveProductReview(int productReviewID)
        {
            return Ok(await _iProductReviewService.RemoveProductReview(productReviewID));
        }
    }
}
