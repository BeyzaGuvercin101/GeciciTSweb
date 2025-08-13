using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class RiskAssessment
{
    public int Id { get; set; }

    [Required]
    public int MaintenanceRequestId { get; set; }

    [Required]
    [Range(1, 3)]
    public int DepartmentCode { get; set; } // 1=Integrity, 2=Maintenance, 3=Production

    [Required]
    [MaxLength(30)]
    public string DepartmentStatus { get; set; } = Domain.Enums.DepartmentStatus.Degerlendirme;

    [MaxLength(500)]
    public string? ReturnReasonCode { get; set; }

    [MaxLength(500)]
    public string? ReturnReasonText { get; set; }

    [MaxLength(500)]
    public string? CancelReasonCode { get; set; }

    [MaxLength(500)]
    public string? CancelReasonText { get; set; }

    [MaxLength(50)]
    public string? RiskCategoryCode { get; set; }

    public DateTime? PlannedTemporaryRepairDate { get; set; }

    public DateTime? PlannedPermanentRepairDate { get; set; }

    [Range(1, 10)]
    public int? CurrentProbability { get; set; }

    [Range(1, 10)]
    public int? CurrentImpact { get; set; }

    public int? CurrentRPN { get; set; } // CurrentProbability * CurrentImpact

    [Range(1, 10)]
    public int? ResidualProbability { get; set; }

    [Range(1, 10)]
    public int? ResidualImpact { get; set; }

    public int? ResidualRPN { get; set; } // ResidualProbability * ResidualImpact

    [MaxLength(500)]
    public string? DepartmentReportNote { get; set; }

    [MaxLength(500)]
    public string? OperationalRiskNote { get; set; }

    public int? ApprovedByUserId { get; set; }

    public DateTime? ApprovedAt { get; set; }

    [Required]
    public int CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties
    public virtual MaintenanceRequest MaintenanceRequest { get; set; } = null!;
    public virtual User CreatedByUser { get; set; } = null!;
    public virtual User? ApprovedByUser { get; set; }

    // Helper method to calculate RPN
    public void CalculateRPN()
    {
        CurrentRPN = CurrentProbability * CurrentImpact;
        ResidualRPN = ResidualProbability * ResidualImpact;
    }
}
