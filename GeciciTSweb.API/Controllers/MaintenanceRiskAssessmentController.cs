using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeciciTSweb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaintenanceRiskAssessmentController : ControllerBase
{
    private readonly IMaintenanceRiskAssessmentService _maintenanceRiskAssessmentService;
    private readonly ILogger<MaintenanceRiskAssessmentController> _logger;

    public MaintenanceRiskAssessmentController(
        IMaintenanceRiskAssessmentService maintenanceRiskAssessmentService,
        ILogger<MaintenanceRiskAssessmentController> logger)
    {
        _maintenanceRiskAssessmentService = maintenanceRiskAssessmentService;
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
    public async Task<ActionResult<RiskAssessmentListDto>> Create([FromBody] CreateMaintenanceRiskAssessmentDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var keycloakSub = GetCurrentUserKeycloakSub();
            var result = await _maintenanceRiskAssessmentService.CreateAsync(createDto, keycloakSub);
            
            _logger.LogInformation("Maintenance risk assessment created by user {UserId} for maintenance request {RequestId}", 
                keycloakSub, createDto.MaintenanceRequestId);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Invalid operation: {Message}", ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating maintenance risk assessment");
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RiskAssessmentListDto>> GetById(int id)
    {
        try
        {
            var result = await _maintenanceRiskAssessmentService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = "Risk değerlendirmesi bulunamadı" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting maintenance risk assessment by id {Id}", id);
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }

    [HttpGet("maintenance-request/{maintenanceRequestId:int}")]
    public async Task<ActionResult<IEnumerable<RiskAssessmentListDto>>> GetByMaintenanceRequestId(int maintenanceRequestId)
    {
        try
        {
            var results = await _maintenanceRiskAssessmentService.GetByMaintenanceRequestIdAsync(maintenanceRequestId);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting maintenance risk assessments for maintenance request {RequestId}", maintenanceRequestId);
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }

    [HttpGet("my-assessments")]
    public async Task<ActionResult<IEnumerable<RiskAssessmentListDto>>> GetMyAssessments()
    {
        try
        {
            var keycloakSub = GetCurrentUserKeycloakSub();
            var results = await _maintenanceRiskAssessmentService.GetByUserAsync(keycloakSub);
            return Ok(results);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user's maintenance risk assessments");
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RiskAssessmentListDto>> Update(int id, [FromBody] CreateMaintenanceRiskAssessmentDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var keycloakSub = GetCurrentUserKeycloakSub();
            var result = await _maintenanceRiskAssessmentService.UpdateAsync(id, updateDto, keycloakSub);
            
            if (result == null)
            {
                return NotFound(new { message = "Risk değerlendirmesi bulunamadı" });
            }

            _logger.LogInformation("Maintenance risk assessment {AssessmentId} updated by user {UserId}", id, keycloakSub);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating maintenance risk assessment {Id}", id);
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var keycloakSub = GetCurrentUserKeycloakSub();
            var success = await _maintenanceRiskAssessmentService.DeleteAsync(id, keycloakSub);
            
            if (!success)
            {
                return NotFound(new { message = "Risk değerlendirmesi bulunamadı" });
            }

            _logger.LogInformation("Maintenance risk assessment {AssessmentId} deleted by user {UserId}", id, keycloakSub);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting maintenance risk assessment {Id}", id);
            return StatusCode(500, new { message = "Bir hata oluştu" });
        }
    }
}
