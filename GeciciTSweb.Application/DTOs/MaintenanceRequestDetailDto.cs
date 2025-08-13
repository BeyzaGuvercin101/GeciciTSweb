using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Application.DTOs
{
    public class MaintenanceRequestDetailDto
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string NotificationNumber { get; set; } = null!;
        public string EquipmentNumber { get; set; } = null!;
        public int TempMaintenanceTypeId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public string? Fluid { get; set; }
        public MaintenanceWorkflowStatus Status { get; set; }
        public bool IsClosed { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Display fields for related entities
        public string UnitName { get; set; } = string.Empty;
        public string ConsoleName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string TempMaintenanceTypeName { get; set; } = string.Empty;
        public string? CreatedByUsername { get; set; }
    }

}
