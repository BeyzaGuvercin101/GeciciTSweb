using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateMaintenanceRequestDto
    {
        public int UnitId { get; set; }
        public string BildirimNumarasi { get; set; } = null!;
        public string EquipmentNumber { get; set; } = null!;
        public int TempMaintenanceTypeId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public string? Fluid { get; set; }
    }
}
