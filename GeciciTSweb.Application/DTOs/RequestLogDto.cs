namespace GeciciTSweb.Application.DTOs;

public class RequestLogDto
{
    public int Id { get; set; }
    public int MaintenanceRequestId { get; set; }
    public int AuthorUserId { get; set; }
    public string ActionType { get; set; } = null!; // "Onay", "Red", "Geri Gönder", "İptal"
    public string? ActionNote { get; set; } // Nedeni / mesajı
    public DateTime CreatedAt { get; set; }
}
