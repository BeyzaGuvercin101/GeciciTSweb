using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!; // Primary user identifier

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
    
    public virtual ICollection<RiskAssessment> CreatedRiskAssessments { get; set; } = new List<RiskAssessment>();
    public virtual ICollection<RiskAssessment> ApprovedRiskAssessments { get; set; } = new List<RiskAssessment>();
}
