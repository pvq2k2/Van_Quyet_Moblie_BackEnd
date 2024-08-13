using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductReviewRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/product-review")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _iProductReviewService;

        public ProductReviewController(IProductReviewService iProductReviewService)
        {
            _iProductReviewService = iProductReviewService;
        }

        [HttpPost("get-all-product-review")]
        public async Task<IActionResult> GetAllProductReview(Pagination pagination)
        {
            return Ok(await _iProductReviewService.GetAllProductReview(pagination));
        }

        [HttpGet("get-product-review-by-id/{productReviewID}")]
        public async Task<IActionResult> GetProductReviewByID([FromRoute] int productReviewID)
        {
            return Ok(await _iProductReviewService.GetProductReviewByID(productReviewID));
        }

        [HttpPost("create-product-review")]
        public async Task<IActionResult> CreateProductReview(CreateProductReviewRequest request)
        {
            return Ok(await _iProductReviewService.CreateProductReview(request));
        }

        [HttpPut("update-product-review/{productReviewID}")]
        public async Task<IActionResult> UpdateProductReview([FromRoute] int productReviewID, UpdateProductReviewRequest request)
        {
            return Ok(await _iProductReviewService.UpdateProductReview(productReviewID, request));
        }

        [HttpDelete("remove-product-review/{productReviewID}")]
        public async Task<IActionResult> RemoveProductReview([FromRoute] int productReviewID)
        {
            return Ok(await _iProductReviewService.RemoveProductReview(productReviewID));
        }
    }
}
