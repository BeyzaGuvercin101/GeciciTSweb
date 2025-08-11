using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string KeycloakSub { get; set; } = null!; // Keycloak user identifier

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties - MaintenanceRequest ile User arasında bağlantı için
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
    public virtual ICollection<RequestLog> RequestLogs { get; set; } = new List<RequestLog>();
}
