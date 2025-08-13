namespace GeciciTSweb.Application.DTOs;

public class RiskAssessmentListDto
{
    public int Id { get; set; }
    public int MaintenanceRequestId { get; set; }
    public int UserId { get; set; }
    public int ImpactBefore { get; set; }
    public int ProbabilityBefore { get; set; }
    public int ImpactAfter { get; set; }
    public int ProbabilityAfter { get; set; }
    public int RPNBefore { get; set; }
    public int RPNAfter { get; set; }
    public string? RiskNote { get; set; }
    public DateTime? PlannedTemporaryRepairDate { get; set; }
    public DateTime? PlannedPermanentRepairDate { get; set; }
    public decimal? CurrentRiskScore { get; set; }
    public decimal? ResidualRiskScore { get; set; }
    public string? RiskCategory { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string AssessmentType { get; set; } = null!; // "Integrity", "Maintenance", "Production"
}
