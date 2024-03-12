using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.CategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _iCategoriesService;

        public CategoriesController(ICategoriesService iCategoriesService)
        {
            _iCategoriesService = iCategoriesService;
        }

        [HttpPost("get-all-categories")]
        public async Task<IActionResult> GetAllCategories(Pagination pagination)
        {
            return Ok(await _iCategoriesService.GetAllCategories(pagination));
        }

        [HttpPost("create-categories")]
        public async Task<IActionResult> CreateCategories([FromBody] CreateCategoriesRequest request)
        {
            return Ok(await _iCategoriesService.CreateCategories(request));
        }

        [HttpGet("get-categories-by-id/{categoriesID}")]
        public async Task<IActionResult> GetCategoriesByID([FromRoute] int categoriesID)
        {
            return Ok(await _iCategoriesService.GetCategoriesByID(categoriesID));
        }

        [HttpGet("get-categories-by-slug/{slug}")]
        public async Task<IActionResult> GetDetailCategoriesBySlug([FromRoute] string slug)
        {
            return Ok(await _iCategoriesService.GetDetailCategoriesBySlug(slug));
        }

        [HttpPut("update-categories/{categoriesID}")]
        public async Task<IActionResult> UpdateCategories([FromRoute] int categoriesID, [FromBody] UpdateCategoriesRequest request)
        {
            return Ok(await _iCategoriesService.UpdateCategories(categoriesID, request));
        }
    }
}
