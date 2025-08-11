using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Application.DTOs;

public class CreateMaintenanceRiskAssessmentDto
{
    [Required]
    public int MaintenanceRequestId { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Impact Before değeri 1-5 arasında olmalıdır")]
    public int ImpactBefore { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Probability Before değeri 1-5 arasında olmalıdır")]
    public int ProbabilityBefore { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Impact After değeri 1-5 arasında olmalıdır")]
    public int ImpactAfter { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Probability After değeri 1-5 arasında olmalıdır")]
    public int ProbabilityAfter { get; set; }

    [MaxLength(1000, ErrorMessage = "Risk notu maksimum 1000 karakter olabilir")]
    public string? RiskNote { get; set; }
}
