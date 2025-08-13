using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeciciTSweb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceRequestsController : ControllerBase
{
    private readonly IMaintenanceRequestService _service;
    private readonly IDashboardService _dashboardService;
    private readonly IMaintenanceRequestSummaryService _summaryService;
    private readonly ILogger<MaintenanceRequestsController> _logger;

    public MaintenanceRequestsController(
        IMaintenanceRequestService service, 
        IDashboardService dashboardService,
        IMaintenanceRequestSummaryService summaryService,
        ILogger<MaintenanceRequestsController> logger)
    {
        _service = service;
        _dashboardService = dashboardService;
        _summaryService = summaryService;
        _logger = logger;
    }

    private string GetCurrentUsername()
    {
        // Header'dan username'i al, yoksa development için default döndür
        var username = Request.Headers["X-Username"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(username))
        {
            // Development için default user döndür
            return "user_2";
        }
        return username;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMaintenanceRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = GetCurrentUsername();
            // MaintenanceRequestCreate
            var id = await _service.CreateAsync(dto, username);
            
            _logger.LogInformation("Maintenance request created by user {UserId} with id {RequestId}", username, id);
            return Ok(new { id });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating maintenance request");
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }
    [HttpPut()]
    public async Task<IActionResult> Update([FromBody] UpdateMaintenanceRequestDto dto)
    {
        try
        {
            var username = GetCurrentUsername();
            var success = await _service.UpdateAsync(dto, username);
            return success ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating maintenance request {Id}", dto.Id);
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }


    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchMaintenanceRequestDto dto)
    {
        try
        {
            // ModelState kontrolü kalsın istersen, ama ID eşitliği kontrolünü kaldır
            if (!ModelState.IsValid) return BadRequest(ModelState);

            dto.Id = id; // <-- kritik
            var username = GetCurrentUsername();
            var success = await _service.PatchAsync(dto, username);

            return success ? NoContent() : NotFound("Maintenance request not found");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            // Forbid yerine doğrudan 403 ObjectResult
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error patching maintenance request {Id}", id);
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }



    [HttpGet]
        [AllowAnonymous] // Test için geçici - normalde sadece authenticated users
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

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetSummary(int id)
        {
            try
            {
                var result = await _summaryService.GetSummaryAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting summary for maintenance request {Id}", id);
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var result = await _dashboardService.GetCardStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics");
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }

        [HttpGet("statistics/openform-by-company")]
        public async Task<IActionResult> GetOpenFormsByCompany()
        {
            try
            {
                var result = await _dashboardService.GetDetailedStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting open forms by company");
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }

        [HttpGet("statistics/closedform-by-company")]
        public async Task<IActionResult> GetClosedFormsByCompany()
        {
            try
            {
                var result = await _dashboardService.GetDetailedStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting closed forms by company");
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var username = GetCurrentUsername();
                var result = await _service.DeleteAsync(id, username);
                
                if (!result) return NotFound();
                
                _logger.LogInformation("Maintenance request {RequestId} deleted by user {UserId}", id, username);
                return NoContent();
            }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            // return Forbid(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }

        catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting maintenance request {Id}", id);
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
        }
    }
