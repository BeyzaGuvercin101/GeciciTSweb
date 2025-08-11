using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Application.DTOs;

public class CreateRequestLogDto
{
    [Required]
    public int MaintenanceRequestId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ActionType { get; set; } = null!; // "Onay", "Red", "Geri Gönder", "İptal"

    [MaxLength(1000)]
    public string? ActionNote { get; set; } // Nedeni / mesajı
}
