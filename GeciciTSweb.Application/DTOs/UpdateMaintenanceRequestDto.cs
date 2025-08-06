using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class UpdateMaintenanceRequestDto
    {
        public int Id { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public string? Fluid { get; set; }
        public string Status { get; set; } = null!;
        public bool IsClosed { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
