using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class MaintenanceRiskAssessment
{
    public int Id { get; set; }

    [Required]
    public int MaintenanceRequestId { get; set; }

    [Required]
    public int UserId { get; set; } // User tablosundaki ID

    [Range(1, 5)]
    public int ImpactBefore { get; set; }

    [Range(1, 5)]
    public int ProbabilityBefore { get; set; }

    [Range(1, 5)]
    public int ImpactAfter { get; set; }

    [Range(1, 5)]
    public int ProbabilityAfter { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int RPNBefore { get; private set; } // ImpactBefore * ProbabilityBefore

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int RPNAfter { get; private set; }  // ImpactAfter * ProbabilityAfter

    [MaxLength(1000)]
    public string? RiskNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties
    public virtual MaintenanceRequest MaintenanceRequest { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
