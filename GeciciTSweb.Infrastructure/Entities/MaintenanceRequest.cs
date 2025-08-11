using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class MaintenanceRequest
{
    public int Id { get; set; }

    [Required]
    public int UnitId { get; set; }

    [Required]
    [MaxLength(50)]
    public string BildirimNumarasi { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string EquipmentNumber { get; set; } = null!;

    [Required]
    public int TempMaintenanceTypeId { get; set; }

    [Range(0, 999.99)]
    public decimal? Temperature { get; set; }

    [Range(0, 999.99)]
    public decimal? Pressure { get; set; }

    [MaxLength(100)]
    public string? Fluid { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = null!;

    public bool IsClosed { get; set; }

    [Required]
    public int CreatedByUserId { get; set; } // User tablosundaki ID

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties
    public virtual Unit Unit { get; set; } = null!;
    public virtual TemporaryMaintenanceType TempMaintenanceType { get; set; } = null!;
    public virtual User CreatedByUser { get; set; } = null!;
    public virtual ICollection<RequestLog> RequestLogs { get; set; } = new List<RequestLog>();
    public virtual ICollection<IntegrityRiskAssessment> IntegrityRiskAssessments { get; set; } = new List<IntegrityRiskAssessment>();
    public virtual ICollection<MaintenanceRiskAssessment> MaintenanceRiskAssessments { get; set; } = new List<MaintenanceRiskAssessment>();
    public virtual ICollection<ProductionRiskAssessment> ProductionRiskAssessments { get; set; } = new List<ProductionRiskAssessment>();
}
