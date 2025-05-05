using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.SizeRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/size")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _iSizeService;

        public SizeController(ISizeService iSizeService)
        {
            _iSizeService = iSizeService;
        }

        [HttpPost("get-all-size")]
        public async Task<IActionResult> GetAllSize(Pagination pagination)
        {
            return Ok(await _iSizeService.GetAllSize(pagination));
        }

        [HttpPost("create-size")]
        public async Task<IActionResult> CreateSize([FromBody] CreateSizeRequest request)
        {
            return Ok(await _iSizeService.CreateSize(request));
        }

        [HttpGet("get-size-by-id/{sizeID}")]
        public async Task<IActionResult> GetSizeByID([FromRoute] int sizeID)
        {
            return Ok(await _iSizeService.GetSizeByID(sizeID));
        }

        [HttpPut("update-size/{sizeID}")]
        public async Task<IActionResult> UpdateSize([FromRoute] int sizeID, [FromBody] UpdateSizeRequest request)
        {
            return Ok(await _iSizeService.UpdateSize(sizeID, request));
        }
    }
}
