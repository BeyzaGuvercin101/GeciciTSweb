using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class Unit
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;


    public string Name { get; set; } = null!;

    public int? ConsoleId { get; set; }

    public virtual Console? Console { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
