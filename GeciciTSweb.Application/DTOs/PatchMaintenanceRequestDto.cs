using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class PatchMaintenanceRequestDto
    {
        public int Id { get; set; }
        
        // Genel Bilgiler - Üretim ekibinin güncelleyebileceği alanlar
        public string? BildirimNumarasi { get; set; }
        public string? EquipmentNumber { get; set; }
        public int? TempMaintenanceTypeId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public string? Fluid { get; set; }
        
        // Form geri gönderilme durumunda güncelleme sebebi
        public string? UpdateReason { get; set; }
    }
}
