using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _iProductService;

        public ProductController(IProductService iProductService)
        {
            _iProductService = iProductService;
        }

        [HttpPost("get-all-product")]
        public async Task<IActionResult> GetAllProduct(Pagination pagination)
        {
            return Ok(await _iProductService.GetAllProduct(pagination));
        }

        [HttpGet("get-product-by-id")]
        public async Task<IActionResult> GetProductByID(int productID)
        {
            return Ok(await _iProductService.GetProductByID(productID));
        }

        [HttpGet("get-product-by-id-and-update-view")]
        public async Task<IActionResult> GetProductByIDAndUpdateView(int productID)
        {
            return Ok(await _iProductService.GetProductByIDAndUpdateView(productID));
        }

        [HttpGet("get-related-products")]
        public async Task<IActionResult> GetRelatedProducts(int productID)
        {
            return Ok(await _iProductService.GetRelatedProducts(productID));
        }

        [HttpGet("get-featured-product")]
        public async Task<IActionResult> GetFeaturedProduct()
        {
            return Ok(await _iProductService.GetFeaturedProduct());
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            return Ok(await _iProductService.CreateProduct(request));
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(int productID, [FromForm] UpdateProductRequest request)
        {
            return Ok(await _iProductService.UpdateProduct(productID, request));
        }

        [HttpDelete("remove-product")]
        public async Task<IActionResult> RemoveProduct(int productID)
        {
            return Ok(await _iProductService.RemoveProduct(productID));
        }
    }
}
