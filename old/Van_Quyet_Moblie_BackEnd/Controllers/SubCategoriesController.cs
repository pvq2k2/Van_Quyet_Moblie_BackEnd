using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SubCategoriesRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/sub-categories")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoriesService _iSubCategoriesService;

        public SubCategoriesController(ISubCategoriesService iSubCategoriesService)
        {
            _iSubCategoriesService = iSubCategoriesService;
        }

        [HttpPost("get-all-sub-categories/{categoriesID}")]
        public async Task<IActionResult> GetAllSubCategories(Pagination pagination, [FromRoute] int categoriesID)
        {
            return Ok(await _iSubCategoriesService.GetAllSubCategories(pagination, categoriesID));
        }

        [HttpPost("create-sub-categories")]
        public async Task<IActionResult> CreateSubCategories([FromForm] CreateSubCategoriesRequest request)
        {
            return Ok(await _iSubCategoriesService.CreateSubCategories(request));
        }

        [HttpGet("get-sub-categories-by-id/{subCategoriesID}")]
        public async Task<IActionResult> GetSubCategoriesByID([FromRoute] int subCategoriesID)
        {
            return Ok(await _iSubCategoriesService.GetSubCategoriesByID(subCategoriesID));
        }

        [HttpPut("update-sub-categories/{subCategoriesID}")]
        public async Task<IActionResult> UpdateSubCategories([FromRoute] int subCategoriesID, [FromForm] UpdateSubCategoriesRequest request)
        {
            return Ok(await _iSubCategoriesService.UpdateSubCategories(subCategoriesID, request));
        }
    }
}
