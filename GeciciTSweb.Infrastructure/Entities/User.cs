using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!; // Primary user 
    public string Password { get; set; }
    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    public virtual ICollection<RiskAssessment> RiskAssessments { get; set; }
}
