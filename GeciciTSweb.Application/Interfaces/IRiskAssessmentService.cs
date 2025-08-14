using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IRiskAssessmentService
    {
        Task<RiskAssessmentDto?> GetByMaintenanceRequestAndDepartmentAsync(int maintenanceRequestId, DepartmentCode departmentCode);
        Task<RiskAssessmentDto> CreateAsync(CreateRiskAssessmentDto dto);
        Task<RiskAssessmentDto> UpdateAsync(int id, UpdateRiskAssessmentDto dto);
        Task<RiskAssessmentDto> ApproveAsync(int id, int approvedByUserId);
        Task<RiskAssessmentDto> ReturnAsync(int id, string reasonCode, string reasonText);
        Task<RiskAssessmentDto> CancelAsync(int id, string reasonCode, string reasonText);
        Task<List<RiskAssessmentDto>> GetByMaintenanceRequestAsync(int maintenanceRequestId);
        Task<List<RiskAssessmentDto>> GetAllAsync();


        Task DeleteAsync(int id);
        
        // Workflow status calculation
        Task UpdateMaintenanceRequestStatusAsync(int maintenanceRequestId);
        Task<MaintenanceWorkflowStatus> CalculateMaintenanceRequestStatusAsync(int maintenanceRequestId);
    }
}
