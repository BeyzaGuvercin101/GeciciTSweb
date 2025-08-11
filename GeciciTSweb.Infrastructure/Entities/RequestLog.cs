using System;
using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class RequestLog
{
    public int Id { get; set; }

    [Required]
    public int MaintenanceRequestId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ActionType { get; set; } = null!; // "Onay", "Red", "Geri Gönder", "İptal"

    [MaxLength(1000)]
    public string? ActionNote { get; set; } // Nedeni / mesajı

    [Required]
    public int AuthorUserId { get; set; } // İşlemi yapan kullanıcı

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation Properties
    public virtual MaintenanceRequest MaintenanceRequest { get; set; } = null!;
    public virtual User AuthorUser { get; set; } = null!;
}
