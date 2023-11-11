using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _iProductService;

        public ProductController(IProductService iProductService)
        {
            _iProductService = iProductService;
        }

        [HttpPost("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct(Pagination pagination)
        {
            return Ok(await _iProductService.GetAllProduct(pagination));
        }

        [HttpGet("GetProductByID")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductByID(int productID)
        {
            return Ok(await _iProductService.GetProductByID(productID));
        }

        [HttpGet("GetProductByIDAndUpdateView")]
        public async Task<IActionResult> GetProductByIDAndUpdateView(int productID)
        {
            return Ok(await _iProductService.GetProductByIDAndUpdateView(productID));
        }

        [HttpGet("GetRelatedProducts")]
        public async Task<IActionResult> GetRelatedProducts(int productID)
        {
            return Ok(await _iProductService.GetRelatedProducts(productID));
        }

        [HttpGet("GetFeaturedProduct")]
        public async Task<IActionResult> GetFeaturedProduct()
        {
            return Ok(await _iProductService.GetFeaturedProduct());
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            return Ok(await _iProductService.CreateProduct(request));
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int productID, [FromForm] UpdateProductRequest request)
        {
            return Ok(await _iProductService.UpdateProduct(productID, request));
        }

        [HttpDelete("RemoveProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct(int productID)
        {
            return Ok(await _iProductService.RemoveProduct(productID));
        }
    }
}
