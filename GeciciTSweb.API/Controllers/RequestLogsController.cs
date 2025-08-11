using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeciciTSweb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestLogsController : ControllerBase
{
    private readonly IRequestLogService _requestLogService;
    private readonly ILogger<RequestLogsController> _logger;

    public RequestLogsController(IRequestLogService requestLogService, ILogger<RequestLogsController> logger)
    {
        _requestLogService = requestLogService;
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

        [HttpGet]
        public async Task<IActionResult> GetByRequestId([FromQuery] int requestId)
        {
            var logs = await _requestLogService.GetByRequestIdAsync(requestId);
            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestLogDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var keycloakSub = GetCurrentUserKeycloakSub();
                var id = await _requestLogService.CreateAsync(dto, keycloakSub);
                
                _logger.LogInformation("Request log created by user {UserId} with id {LogId}", keycloakSub, id);
                return Ok(new { id });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating request log");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var keycloakSub = GetCurrentUserKeycloakSub();
                var result = await _requestLogService.SoftDeleteAsync(id, keycloakSub);
                
                if (!result) return NotFound();
                
                _logger.LogInformation("Request log {LogId} deleted by user {UserId}", id, keycloakSub);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access: {Message}", ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting request log {Id}", id);
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
        }
    }
