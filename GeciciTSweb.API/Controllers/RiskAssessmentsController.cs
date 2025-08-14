using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GeciciTSweb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskAssessmentsController : ControllerBase
    {
        private readonly IRiskAssessmentService _riskAssessmentService;

        public RiskAssessmentsController(IRiskAssessmentService riskAssessmentService)
        {
            _riskAssessmentService = riskAssessmentService;
        }

        /// <summary>
        /// Get risk assessment by maintenance request and department
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<RiskAssessmentDto>> GetByMaintenanceRequestAndDepartment([FromQuery] int requestId, [FromQuery] int department)
        {
            if (!Enum.IsDefined(typeof(DepartmentCode), department))
            {
                return BadRequest("Invalid department code");
            }

            var result = await _riskAssessmentService.GetByMaintenanceRequestAndDepartmentAsync(requestId, (DepartmentCode)department);

            if (result == null)
            {
                return NoContent(); // 204 - indicates department tab should show "Start" button
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all risk assessments for a maintenance request
        /// </summary>
        [HttpGet("by-request/{requestId}")]
        public async Task<ActionResult<List<RiskAssessmentDto>>> GetByMaintenanceRequest(int requestId)
        {
            var result = await _riskAssessmentService.GetByMaintenanceRequestAsync(requestId);

            if (result == null || result.Count == 0)
            {
                return NoContent(); // 204 - hiç kayýt yoksa
            }

            return Ok(result);
        }
        /// <summary>
        /// Get all risk assessments
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<List<RiskAssessmentDto>>> GetAll()
        {
            var result = await _riskAssessmentService.GetAllAsync();

            if (result == null || result.Count == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }




        /// <summary>
        /// Create new risk assessment (start department evaluation)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RiskAssessmentDto>> Create([FromBody] CreateRiskAssessmentDto dto)
        {
            try
            {
                var result = await _riskAssessmentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByMaintenanceRequestAndDepartment), 
                    new { requestId = dto.MaintenanceRequestId, department = dto.DepartmentCode }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = new { code = "DuplicateAssessment", message = ex.Message } });
            }
        }

        /// <summary>
        /// Update risk assessment (risk values, dates, notes, status)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RiskAssessmentDto>> Update(int id, [FromBody] UpdateRiskAssessmentDto dto)
        {
            try
            {
                var result = await _riskAssessmentService.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = new { code = "NotFound", message = ex.Message } });
            }
        }

        /// <summary>
        /// Approve risk assessment
        /// </summary>
        [HttpPost("{id}/approve")]
        public async Task<ActionResult<RiskAssessmentDto>> Approve(int id, [FromBody] ApproveRequestDto dto)
        {
            try
            {
                var result = await _riskAssessmentService.ApproveAsync(id, dto.ApprovedByUserId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = new { code = "NotFound", message = ex.Message } });
            }
        }

        /// <summary>
        /// Return risk assessment with reason
        /// </summary>
        [HttpPost("{id}/return")]
        public async Task<ActionResult<RiskAssessmentDto>> Return(int id, [FromBody] ReturnRequestDto dto)
        {
            try
            {
                var result = await _riskAssessmentService.ReturnAsync(id, dto.ReasonCode, dto.ReasonText);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = new { code = "NotFound", message = ex.Message } });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = new { code = "ValidationError", message = ex.Message } });
            }
        }

        /// <summary>
        /// Cancel risk assessment with reason
        /// </summary>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<RiskAssessmentDto>> Cancel(int id, [FromBody] CancelRequestDto dto)
        {
            try
            {
                var result = await _riskAssessmentService.CancelAsync(id, dto.ReasonCode, dto.ReasonText);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = new { code = "NotFound", message = ex.Message } });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = new { code = "ValidationError", message = ex.Message } });
            }
        }

        /// <summary>
        /// Soft delete risk assessment
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _riskAssessmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = new { code = "NotFound", message = ex.Message } });
            }
        }
    }

    // Request DTOs for actions
    public class ApproveRequestDto
    {
        public int ApprovedByUserId { get; set; }
    }

    public class ReturnRequestDto
    {
        public string ReasonCode { get; set; } = null!;
        public string ReasonText { get; set; } = null!;
    }

    public class CancelRequestDto
    {
        public string ReasonCode { get; set; } = null!;
        public string ReasonText { get; set; } = null!;
    }
}
