using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class TemporaryMaintenanceType
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;

    public string Name { get; set; } = null!;

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
}
