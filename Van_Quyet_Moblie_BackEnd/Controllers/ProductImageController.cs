using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/product-image")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _iProductImageService;

        public ProductImageController(IProductImageService iProductImageService)
        {
            _iProductImageService = iProductImageService;
        }

        [HttpPost("get-all-product-image/{productID}")]
        public async Task<IActionResult> GetAllProductImage(Pagination pagination, [FromRoute] int productID)
        {
            return Ok(await _iProductImageService.GetAllProductImage(pagination, productID));
        }

        [HttpGet("get-product-image-by-id/{productImage}")]
        public async Task<IActionResult> GetProductImageByID([FromRoute] int productImage)
        {
            return Ok(await _iProductImageService.GetProductImageByID(productImage));
        }

        [HttpGet("get-update-product-image-by-id/{productImageID}")]
        public async Task<IActionResult> GetUpdateProductImageByID([FromRoute] int productImageID)
        {
            return Ok(await _iProductImageService.GetUpdateProductImageByID(productImageID));
        }

        [HttpPost("create-product-image")]
        public async Task<IActionResult> CreateProductImage([FromForm] CreateProductImageRequest request)
        {
            return Ok(await _iProductImageService.CreateProductImage(request));
        }

        [HttpPut("update-product-image/{productImageID}")]
        public async Task<IActionResult> UpdateProductImage([FromRoute] int productImageID, [FromForm] UpdateProductImageRequest request)
        {
            return Ok(await _iProductImageService.UpdateProductImage(productImageID, request));
        }

        [HttpDelete("remove-product-image/{productImageID}")]
        public async Task<IActionResult> RemoveProductImage([FromRoute] int productImageID)
        {
            return Ok(await _iProductImageService.RemoveProductImage(productImageID));
        }
    }
}
