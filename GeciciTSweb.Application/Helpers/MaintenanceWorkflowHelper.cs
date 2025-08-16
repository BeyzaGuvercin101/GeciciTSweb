using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeciciTSweb.Domain.Enums;

namespace GeciciTSweb.Application.Helpers
{
    public static class MaintenanceWorkflowHelper
    {
        public static bool IsClosed(MaintenanceWorkflowStatus status)
        {
            return status == MaintenanceWorkflowStatus.Onaylandi || status == MaintenanceWorkflowStatus.IptalEdildi;
        }

        public static bool IsOpen(MaintenanceWorkflowStatus status)
        {
            return !IsClosed(status);
        }


    }
}

