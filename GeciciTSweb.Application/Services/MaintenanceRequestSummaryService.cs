using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GeciciTSweb.Application.Services
{
    public class MaintenanceRequestSummaryService : IMaintenanceRequestSummaryService
    {
        private readonly GeciciTSwebDbContext _context;
        private readonly IMapper _mapper;

        public MaintenanceRequestSummaryService(GeciciTSwebDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MaintenanceRequestSummaryDto?> GetSummaryAsync(int maintenanceRequestId)
        {
            var maintenanceRequest = await _context.MaintenanceRequests
                .Include(mr => mr.Unit)
                    .ThenInclude(u => u.Console)
                        .ThenInclude(c => c.Company)
                .Include(mr => mr.TempMaintenanceType)
                .Include(mr => mr.CreatedByUser)
                .Include(mr => mr.RiskAssessments)
                    .ThenInclude(ra => ra.CreatedByUser)
                .Include(mr => mr.RiskAssessments)
                    .ThenInclude(ra => ra.ApprovedByUser)
                .FirstOrDefaultAsync(mr => mr.Id == maintenanceRequestId);

            if (maintenanceRequest == null)
                return null;

            var summary = new MaintenanceRequestSummaryDto
            {
                Request = _mapper.Map<MaintenanceRequestDetailDto>(maintenanceRequest),
                Steps = CalculateSteps(maintenanceRequest),
                Departments = await CalculateDepartmentSummaries(maintenanceRequest),
                Permissions = CalculatePermissions() // TODO: Implement based on user claims
            };

            return summary;
        }

        private List<StepDto> CalculateSteps(Infrastructure.Entities.MaintenanceRequest maintenanceRequest)
        {
            var status = maintenanceRequest.Status;

            return new List<StepDto>
            {
                new StepDto
                {
                    Name = "GenelBilgiler",
                    State = "Completed" // Always completed if MR exists
                },
                new StepDto
                {
                    Name = "RiskDegerlendirme",
                    State = CalculateRiskEvaluationState(status)
                },
                new StepDto
                {
                    Name = "Onay",
                    State = CalculateApprovalState(status)
                }
            };
        }

        private string CalculateRiskEvaluationState(MaintenanceWorkflowStatus status)
        {
            return status switch
            {
                MaintenanceWorkflowStatus.YeniTalep => "Pending",
                MaintenanceWorkflowStatus.ButunlukDegerlendirmesi or 
                MaintenanceWorkflowStatus.BakimDegerlendirmesi or 
                MaintenanceWorkflowStatus.UretimKontrolu => "InProgress",
                MaintenanceWorkflowStatus.OnayBekliyor or 
                MaintenanceWorkflowStatus.Onaylandi => "Completed",
                _ => "Pending"
            };
        }

        private string CalculateApprovalState(MaintenanceWorkflowStatus status)
        {
            return status switch
            {
                MaintenanceWorkflowStatus.OnayBekliyor => "InProgress",
                MaintenanceWorkflowStatus.Onaylandi => "Completed",
                _ => "Pending"
            };
        }

        private async Task<List<DepartmentSummaryDto>> CalculateDepartmentSummaries(Infrastructure.Entities.MaintenanceRequest maintenanceRequest)
        {
            var departments = new List<DepartmentSummaryDto>();

            // Create department summaries for all three departments
            foreach (DepartmentCode deptCode in Enum.GetValues<DepartmentCode>())
            {
                var riskAssessment = maintenanceRequest.RiskAssessments
                    .FirstOrDefault(ra => ra.DepartmentCode == (int)deptCode);

                var deptSummary = new DepartmentSummaryDto
                {
                    Code = deptCode.ToString(),
                    Status = riskAssessment?.DepartmentStatus ?? DepartmentStatus.Degerlendirme,
                    CurrentRPN = riskAssessment?.CurrentRPN,
                    ResidualRPN = riskAssessment?.ResidualRPN,
                    PlannedTemporaryRepairDate = riskAssessment?.PlannedTemporaryRepairDate,
                    PlannedPermanentRepairDate = riskAssessment?.PlannedPermanentRepairDate,
                    ReturnReasonText = riskAssessment?.ReturnReasonText,
                    CancelReasonText = riskAssessment?.CancelReasonText,
                    ApprovedBy = riskAssessment?.ApprovedByUser != null ? new UserDto 
                    { 
                        Id = riskAssessment.ApprovedByUser.Id, 
                        Username = riskAssessment.ApprovedByUser.Username 
                    } : null,
                    ApprovedAt = riskAssessment?.ApprovedAt
                };

                departments.Add(deptSummary);
            }

            return departments;
        }

        private PermissionsDto CalculatePermissions()
        {
            // TODO: Implement based on user claims/roles
            // For now, return default permissions
            return new PermissionsDto
            {
                CanEditIntegrity = true,
                CanEditMaintenance = true,
                CanEditProduction = true
            };
        }
    }
}
