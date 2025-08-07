using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class DashboardCardDto
    {
        public int ApprovedCount { get; set; }
        public int ReturnedCount { get; set; }
        public int RejectedCount { get; set; }
        public int InProgressCount { get; set; }
    }
}

