
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
        public string Status { get; set; } = null!;
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
        public MaintenanceWorkflowStatus WorkflowStatus { get; set; } // Enum ile bağlanacak


    }
}
