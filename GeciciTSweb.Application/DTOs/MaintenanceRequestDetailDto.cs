using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class MaintenanceRequestDetailDto
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string BildirimNumarasi { get; set; } = null!;
        public string EquipmentNumber { get; set; } = null!;
        public int TempMaintenanceTypeId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public string? Fluid { get; set; }
        public string Status { get; set; } = null!;
        public bool IsClosed { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
