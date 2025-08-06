using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class RequestLog
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;


    public int MaintenanceRequestId { get; set; }

    public int AuthorUserId { get; set; }

    public string LogType { get; set; } = null!;

    public string? Reason { get; set; }

    public string? Comment { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User AuthorUser { get; set; } = null!;

    public virtual MaintenanceRequest MaintenanceRequest { get; set; } = null!;
}
