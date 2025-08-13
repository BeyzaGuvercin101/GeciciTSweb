using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class Unit
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;


    public string Name { get; set; } = null!;

    public int? ConsoleId { get; set; }

    [ForeignKey("ConsoleId")]
    public virtual Console? Console { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
}
