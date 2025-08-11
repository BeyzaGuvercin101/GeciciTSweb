using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces;

public interface IIntegrityRiskAssessmentService
{
    Task<RiskAssessmentListDto> CreateAsync(CreateIntegrityRiskAssessmentDto createDto, string keycloakSub);
    Task<RiskAssessmentListDto?> GetByIdAsync(int id);
    Task<IEnumerable<RiskAssessmentListDto>> GetByMaintenanceRequestIdAsync(int maintenanceRequestId);
    Task<RiskAssessmentListDto?> UpdateAsync(int id, CreateIntegrityRiskAssessmentDto updateDto, string keycloakSub);
    Task<bool> DeleteAsync(int id, string keycloakSub);
    Task<IEnumerable<RiskAssessmentListDto>> GetByUserAsync(string keycloakSub);
}
