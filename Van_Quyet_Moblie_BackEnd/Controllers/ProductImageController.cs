using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _iProductImageService;

        public ProductImageController(IProductImageService iProductImageService)
        {
            _iProductImageService = iProductImageService;
        }

        [HttpPost("GetAllProductImage")]
        public async Task<IActionResult> GetAllProductImage(Pagination pagination)
        {
            return Ok(await _iProductImageService.GetAllProductImage(pagination));
        }

        [HttpGet("GetProductImageByID")]
        public async Task<IActionResult> GetProductImageByID(int productID)
        {
            return Ok(await _iProductImageService.GetProductImageByID(productID));
        }

        [HttpPost("CreateProductImage")]
        public async Task<IActionResult> CreateProductImage([FromForm] CreateProductImageRequest request)
        {
            return Ok(await _iProductImageService.CreateProductImage(request));
        }

        [HttpPut("UpdateProductImage")]
        public async Task<IActionResult> UpdateProductImage(int productImageID, [FromForm] UpdateProductImageRequest request)
        {
            return Ok(await _iProductImageService.UpdateProductImage(productImageID, request));
        }

        [HttpDelete("RemoveProductImage")]
        public async Task<IActionResult> RemoveProductImage(int productImageID)
        {
            return Ok(await _iProductImageService.RemoveProductImage(productImageID));
        }
    }
}
