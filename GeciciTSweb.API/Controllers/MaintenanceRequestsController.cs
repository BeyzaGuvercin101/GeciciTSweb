using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeciciTSweb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly IMaintenanceRequestService _service;
        private readonly IDashboardService _dashboardService;

        public MaintenanceRequestsController(IMaintenanceRequestService service, IDashboardService dashboardService)
        {
            _service = service;
            _dashboardService = dashboardService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMaintenanceRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMaintenanceRequestDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var success = await _service.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _dashboardService.GetCardStatisticsAsync();
            return Ok(result);
        }

        [HttpGet("statistics/openform-by-company")]
        public async Task<IActionResult> GetOpenFormsByCompany()
        {
            // Bu method'u DashboardService'e ekleyeceğiz
            return Ok(new { message = "To be implemented" });
        }

        [HttpGet("statistics/closedform-by-company")]
        public async Task<IActionResult> GetClosedFormsByCompany()
        {
            // Bu method'u DashboardService'e ekleyeceğiz
            return Ok(new { message = "To be implemented" });
        }
    }

}
