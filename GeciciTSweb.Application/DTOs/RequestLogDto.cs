using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class RequestLogDto
    {
        public int Id { get; set; }
        public int MaintenanceRequestId { get; set; }
        public int AuthorUserId { get; set; }
        public string LogType { get; set; } = null!;
        public string? Reason { get; set; }
        public string? Comment { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
