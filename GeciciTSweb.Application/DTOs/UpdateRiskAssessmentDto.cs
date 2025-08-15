using GeciciTSweb.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Application.DTOs
{
    public class UpdateRiskAssessmentDto
    {
        [MaxLength(50)]
        public string? RiskCategoryCode { get; set; }

        public int? DepartmentCode { get; set; }

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

        [MaxLength(30)]
        public string? DepartmentStatus { get; set; }
    }
}
