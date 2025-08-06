using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string KeycloakUserId { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public int? RoleId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual ICollection<RequestLog> RequestLogs { get; set; } = new List<RequestLog>();

    public virtual Role? Role { get; set; }
}
