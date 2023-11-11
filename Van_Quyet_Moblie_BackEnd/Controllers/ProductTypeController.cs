using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyTrungTam_API.Helper;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductTypeRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _iProductTypeService;

        public ProductTypeController(IProductTypeService iProductTypeService)
        {
            _iProductTypeService = iProductTypeService;
        }

        [HttpPost("GetAllProductType")]
        public async Task<IActionResult> GetAllProductType(Pagination pagination)
        {
            return Ok(await _iProductTypeService.GetAllProductType(pagination));
        }

        [HttpGet("GetProductTypeByID")]
        public async Task<IActionResult> GetProductByID(int productID)
        {
            return Ok(await _iProductTypeService.GetProductTypeByID(productID));
        }

        [HttpPost("CreateProductType")]
        public async Task<IActionResult> CreateProductType([FromForm] CreateProductTypeRequest request)
        {
            return Ok(await _iProductTypeService.CreateProductType(request));
        }

        [HttpPut("UpdateProductType")]
        public async Task<IActionResult> UpdateProductType(int productTypeID, [FromForm] UpdateProductTypeRequest request)
        {
            return Ok(await _iProductTypeService.UpdateProductType(productTypeID, request));
        }

        [HttpDelete("RemoveProductType")]
        public async Task<IActionResult> RemoveProductType(int productTypeID)
        {
            return Ok(await _iProductTypeService.RemoveProductType(productTypeID));
        }
    }
}
