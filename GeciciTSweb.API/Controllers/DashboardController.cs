using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous] // Test için geçici
        public async Task<IActionResult> GetCardStats()
        {
            var result = await _dashboardService.GetCardStatisticsAsync();
            return Ok(result);
        }

        [HttpGet("detailed-stats")]
        [AllowAnonymous] // Test için geçici
        public async Task<IActionResult> GetDetailedStats()
        {
            try
            {
                var result = await _dashboardService.GetDetailedStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

