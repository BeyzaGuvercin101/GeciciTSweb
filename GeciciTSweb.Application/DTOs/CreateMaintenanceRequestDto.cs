using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Application.DTOs;

public class CreateMaintenanceRequestDto
{
    [Required]
    public int UnitId { get; set; }

    [Required]
    [MaxLength(50)]
    public string NotificationNumber { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string EquipmentNumber { get; set; } = null!;

    [Required]
    public int TempMaintenanceTypeId { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Pressure { get; set; }

    [MaxLength(100)]
    public string? Fluid { get; set; }
}
