using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.DecentralizationRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/decentralization")]
    [ApiController]
    public class DecentralizationController : ControllerBase
    {
        private readonly IDecentralizationService _iDecentralizationService;

        public DecentralizationController(IDecentralizationService iDecentralizationService)
        {
            _iDecentralizationService = iDecentralizationService;
        }

        [HttpPost("get-all-decentralization")]
        public async Task<IActionResult> GetAllDecentralization(Pagination pagination)
        {
            return Ok(await _iDecentralizationService.GetAllDecentralization(pagination));
        }

        [HttpGet("get-decentralization-by-id/{decentralizationID}")]
        public async Task<IActionResult> GetDecentralizationByID([FromRoute] int decentralizationID)
        {
            return Ok(await _iDecentralizationService.GetDecentralizationByID(decentralizationID));
        }

        [HttpPost("create-decentralization")]
        public async Task<IActionResult> CreateDecentralization(CreateDecentralizationRequest request)
        {
            return Ok(await _iDecentralizationService.CreateDecentralization(request));
        }

        [HttpPut("update-decentralization/{decentralizationID}")]
        public async Task<IActionResult> UpdateDecentralization([FromRoute] int decentralizationID, UpdateDecentralizationRequest request)
        {
            return Ok(await _iDecentralizationService.UpdateDecentralization(decentralizationID, request));
        }

        [HttpDelete("remove-decentralization/{decentralizationID}")]
        public async Task<IActionResult> RemoveDecentralization([FromRoute] int decentralizationID)
        {
            return Ok(await _iDecentralizationService.RemoveDecentralization(decentralizationID));
        }
    }
}
