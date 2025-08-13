using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class MaintenanceRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UnitId { get; set; }

    [Required]
    [MaxLength(50)]
    public string NotificationNumber { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string EquipmentNumber { get; set; } = null!;

    [Required]
    public int TempMaintenanceTypeId { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Pressure { get; set; }

    [MaxLength(100)]
    public string? Fluid { get; set; }

    [Required]
    public MaintenanceWorkflowStatus Status { get; set; }

    public bool IsClosed { get; set; }

    [Required]
    public int CreatedByUserId { get; set; } // User tablosundaki ID

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties
    [ForeignKey("UnitId")]
    public virtual Unit Unit { get; set; }
    [ForeignKey("TempMaintenanceTypeId")]
    public virtual TemporaryMaintenanceType TempMaintenanceType { get; set; }
    
    public virtual ICollection<RiskAssessment> RiskAssessment { get; set; }
}
