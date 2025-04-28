using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/slides")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly ISlidesService _iSlidesService;

        public SlidesController(ISlidesService iSlidesService)
        {
            _iSlidesService = iSlidesService;
        }

        [HttpPost("get-all-slides")]
        public async Task<IActionResult> GetAllSlides(Pagination pagination)
        {
            return Ok(await _iSlidesService.GetAllSlides(pagination));
        }

        [HttpGet("get-active-slides")]
        public async Task<IActionResult> GetActiveSlides()
        {
            return Ok(await _iSlidesService.GetActiveSlides());
        }

        [HttpGet("get-slides-by-id/{slidesID}")]
        public async Task<IActionResult> GetSlidesByID([FromRoute] int slidesID)
        {
            return Ok(await _iSlidesService.GetSlidesByID(slidesID));
        }

        [HttpPost("create-slides")]
        public async Task<IActionResult> CreateSlides([FromForm] SlidesRequest request)
        {
            return Ok(await _iSlidesService.CreateSlides(request));
        }

        [HttpPut("update-slides/{slidesID}")]
        public async Task<IActionResult> UpdateSlides([FromRoute] int slidesID, [FromForm] SlidesRequest request)
        {
            return Ok(await _iSlidesService.UpdateSlides(slidesID, request));
        }

        [HttpDelete("remove-slides/{slidesID}")]
        public async Task<IActionResult> RemoveSlides([FromRoute] int slidesID)
        {
            return Ok(await _iSlidesService.RemoveSlides(slidesID));
        }
    }
}
