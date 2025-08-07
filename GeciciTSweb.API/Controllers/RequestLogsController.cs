using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeciciTSweb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestLogsController : ControllerBase
    {
        private readonly IRequestLogService _requestLogService;

        public RequestLogsController(IRequestLogService requestLogService)
        {
            _requestLogService = requestLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByRequestId([FromQuery] int requestId)
        {
            var logs = await _requestLogService.GetByRequestIdAsync(requestId);
            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestLogDto dto)
        {
            var id = await _requestLogService.CreateAsync(dto);
            return Ok(new { id });
        }
    }
}
