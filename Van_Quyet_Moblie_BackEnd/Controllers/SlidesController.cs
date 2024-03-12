using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SlidesController : ControllerBase
    {
        private readonly ISlidesService _iSlidesService;

        public SlidesController(ISlidesService iSlidesService)
        {
            _iSlidesService = iSlidesService;
        }

        [HttpPost("GetAllSlides")]
        public async Task<IActionResult> GetAllSlides(Pagination pagination)
        {
            return Ok(await _iSlidesService.GetAllSlides(pagination));
        }

        [HttpGet("GetActiveSlides")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveSlides()
        {
            return Ok(await _iSlidesService.GetActiveSlides());
        }

        [HttpGet("GetSlidesByID")]
        public async Task<IActionResult> GetSlidesByID(int slidesID)
        {
            return Ok(await _iSlidesService.GetSlidesByID(slidesID));
        }

        [HttpPost("CreateSlides")]
        public async Task<IActionResult> CreateSlides([FromForm] CreateSlidesRequest request)
        {
            return Ok(await _iSlidesService.CreateSlides(request));
        }

        [HttpPut("UpdateSlides")]
        public async Task<IActionResult> UpdateSlides(int slidesID, [FromForm] UpdateSlidesRequest request)
        {
            return Ok(await _iSlidesService.UpdateSlides(slidesID, request));
        }

        [HttpDelete("RemoveSlides")]
        public async Task<IActionResult> RemoveSlides(int slidesID)
        {
            return Ok(await _iSlidesService.RemoveSlides(slidesID));
        }
    }
}
