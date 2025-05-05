using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ColorRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/color")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _iColorService;

        public ColorController(IColorService iColorService)
        {
            _iColorService = iColorService;
        }

        [HttpPost("get-all-color")]
        public async Task<IActionResult> GetAllColor(Pagination pagination)
        {
            return Ok(await _iColorService.GetAllColor(pagination));
        }

        [HttpPost("create-color")]
        public async Task<IActionResult> CreateColor([FromBody] CreateColorRequest request)
        {
            return Ok(await _iColorService.CreateColor(request));
        }

        [HttpGet("get-color-by-id/{colorID}")]
        public async Task<IActionResult> GetColorByID([FromRoute] int colorID)
        {
            return Ok(await _iColorService.GetColorByID(colorID));
        }

        [HttpPut("update-color/{colorID}")]
        public async Task<IActionResult> UpdateColor([FromRoute] int colorID, [FromBody] UpdateColorRequest request)
        {
            return Ok(await _iColorService.UpdateColor(colorID, request));
        }
    }
}
