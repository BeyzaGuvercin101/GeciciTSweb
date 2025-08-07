using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class MaintenanceRequest
{
    public int Id { get; set; }

    public int UnitId { get; set; }

    public string BildirimNumarasi { get; set; } = null!;

    public string EquipmentNumber { get; set; } = null!;

    public int TempMaintenanceTypeId { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Pressure { get; set; }

    public string? Fluid { get; set; }

    public string Status { get; set; } = null!;

    public bool IsClosed { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public virtual Unit Unit { get; set; } = null;

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<RequestLog> RequestLogs { get; set; } = new List<RequestLog>();

    public virtual TemporaryMaintenanceType TempMaintenanceType { get; set; } = null!;

    
    
}
