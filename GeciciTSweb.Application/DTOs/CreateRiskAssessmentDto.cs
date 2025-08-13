using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateRiskAssessmentDto
    {
        [Required]
        public int MaintenanceRequestId { get; set; }

        [Required]
        [Range(1, 3)]
        public int DepartmentCode { get; set; }

        [MaxLength(50)]
        public string? RiskCategoryCode { get; set; }

        public DateTime? PlannedTemporaryRepairDate { get; set; }

        public DateTime? PlannedPermanentRepairDate { get; set; }

        [Range(1, 10)]
        public int? CurrentProbability { get; set; }

        [Range(1, 10)]
        public int? CurrentImpact { get; set; }

        [Range(1, 10)]
        public int? ResidualProbability { get; set; }

        [Range(1, 10)]
        public int? ResidualImpact { get; set; }

        [MaxLength(500)]
        public string? DepartmentReportNote { get; set; }

        [MaxLength(500)]
        public string? OperationalRiskNote { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}
