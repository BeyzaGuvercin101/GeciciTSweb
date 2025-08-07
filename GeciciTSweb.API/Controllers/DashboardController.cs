using GeciciTSweb.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeciciTSweb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("cards")]
        public async Task<IActionResult> GetCardStats()
        {
            var result = await _dashboardService.GetCardStatisticsAsync();
            return Ok(result);
        }
    }
}

