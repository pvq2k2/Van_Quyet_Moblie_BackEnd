using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductAttributeRequest;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/product-attribute")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IProductAttributeService _iProductAttributeService;

        public ProductAttributeController(IProductAttributeService iProductAttributeService)
        {
            _iProductAttributeService = iProductAttributeService;
        }

        [HttpPost("get-all-product-attribute/{productID}")]
        public async Task<IActionResult> GetAllProductAttribute(Pagination pagination, [FromRoute] int productID)
        {
            return Ok(await _iProductAttributeService.GetAllProductAttribute(pagination, productID));
        }

        [HttpGet("get-update-product-attribute-by-id/{productAttributeID}")]
        public async Task<IActionResult> GetUpdateProductAttributeByID([FromRoute] int productAttributeID)
        {
            return Ok(await _iProductAttributeService.GetUpdateProductAttributeByID(productAttributeID));
        }

        [HttpPost("create-product-attribute")]
        public async Task<IActionResult> CreateProductAttribute(CreateProductAttributeRequest request)
        {
            return Ok(await _iProductAttributeService.CreateProductAttribute(request));
        }

        [HttpPut("update-product-attribute/{productAttributeID}")]
        public async Task<IActionResult> UpdateProductAttribute([FromRoute] int productAttributeID, UpdateProductAttributeRequest request)
        {
            return Ok(await _iProductAttributeService.UpdateProductAttribute(productAttributeID, request));
        }

        [HttpDelete("remove-product-attribute/{productAttributeID}")]
        public async Task<IActionResult> RemoveProductAttribute([FromRoute] int productAttributeID)
        {
            return Ok(await _iProductAttributeService.RemoveProductAttribute(productAttributeID));
        }
    }
}
