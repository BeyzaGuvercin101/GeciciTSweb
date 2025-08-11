using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeciciTSweb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Tüm endpoint'ler için authorization gerekli
public class MaintenanceRequestsController : ControllerBase
{
    private readonly IMaintenanceRequestService _service;
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<MaintenanceRequestsController> _logger;

    public MaintenanceRequestsController(
        IMaintenanceRequestService service, 
        IDashboardService dashboardService,
        ILogger<MaintenanceRequestsController> logger)
    {
        _service = service;
        _dashboardService = dashboardService;
        _logger = logger;
    }

    private string GetCurrentUserKeycloakSub()
    {
        var keycloakSub = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(keycloakSub))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }
        return keycloakSub;
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

            var keycloakSub = GetCurrentUserKeycloakSub();
            var id = await _service.CreateAsync(dto, keycloakSub);
            
            _logger.LogInformation("Maintenance request created by user {UserId} with id {RequestId}", keycloakSub, id);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMaintenanceRequestDto dto)
        {
            try
            {
                if (id != dto.Id) return BadRequest();
                
                var keycloakSub = GetCurrentUserKeycloakSub();
                var success = await _service.UpdateAsync(dto, keycloakSub);
                
                _logger.LogInformation("Maintenance request {RequestId} updated by user {UserId}", id, keycloakSub);
                return success ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating maintenance request {Id}", id);
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, PatchMaintenanceRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != dto.Id) return BadRequest("ID mismatch");
                
                var keycloakSub = GetCurrentUserKeycloakSub();
                var success = await _service.PatchAsync(dto, keycloakSub);
                if (!success) return NotFound("Maintenance request not found");

                _logger.LogInformation("Maintenance request {RequestId} patched by user {UserId}", id, keycloakSub);
                return NoContent(); // 204 No Content - PATCH başarılı
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching maintenance request {Id}", id);
                return BadRequest(new { error = ex.Message });
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var keycloakSub = GetCurrentUserKeycloakSub();
                var result = await _service.DeleteAsync(id, keycloakSub);
                
                if (!result) return NotFound();
                
                _logger.LogInformation("Maintenance request {RequestId} deleted by user {UserId}", id, keycloakSub);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting maintenance request {Id}", id);
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
        }
    }
