using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Domain.Enums
{
    public enum MaintenanceWorkflowStatus
    {
        YeniTalep,
        ButunlukDegerlendirmesi,
        BakimDegerlendirmesi,
        UretimKontrolu,
        OnayBekliyor,
        Onaylandi,
        IptalEdildi,
        GeriGonderildi
    }
}
