using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Application.DTOs
{
    public class RiskAssessmentDto
    {
        public int Id { get; set; }
        public int MaintenanceRequestId { get; set; }
        public int DepartmentCode { get; set; }
        public string DepartmentStatus { get; set; } = null!;
        public string? ReturnReasonCode { get; set; }
        public string? ReturnReasonText { get; set; }
        public string? CancelReasonCode { get; set; }
        public string? CancelReasonText { get; set; }
        public string? RiskCategoryCode { get; set; }
        public DateTime? PlannedTemporaryRepairDate { get; set; }
        public DateTime? PlannedPermanentRepairDate { get; set; }
        public int? CurrentProbability { get; set; }
        public int? CurrentImpact { get; set; }
        public int? CurrentRPN { get; set; }
        public int? ResidualProbability { get; set; }
        public int? ResidualImpact { get; set; }
        public int? ResidualRPN { get; set; }
        public string? DepartmentReportNote { get; set; }
        public string? OperationalRiskNote { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties for display
        public string? CreatedByUsername { get; set; }
        public string? ApprovedByUsername { get; set; }
        public string DepartmentName => ((DepartmentCode)DepartmentCode).ToString();
    }
}
