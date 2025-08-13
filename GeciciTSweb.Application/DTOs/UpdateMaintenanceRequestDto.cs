using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Application.DTOs
{
    public class UpdateMaintenanceRequestDto
    {
        public int Id { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public int TempMaintenanceTypeId { get; set; }
        public string? Fluid { get; set; }
        public MaintenanceWorkflowStatus Status { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
