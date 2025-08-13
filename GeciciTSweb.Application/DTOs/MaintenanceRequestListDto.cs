
using GeciciTSweb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class MaintenanceRequestListDto
    {
        public int Id { get; set; }
        public string BildirimNumarasi { get; set; } = null!;
        public MaintenanceWorkflowStatus Status { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public string? Fluid { get; set; }
        public string EquipmentNumber { get; set; } = null!;
        
        // Display fields for better list view
        public string UnitName { get; set; } = string.Empty;
        public string ConsoleName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string TempMaintenanceTypeName { get; set; } = string.Empty;
        public int TempMaintenanceTypeId { get; set; }
    }
}
