using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Domain.Enums
{
    public enum MaintenanceWorkflowStatus
    {
        YeniTalep = 0,
        ButunlukDegerlendirmesi = 1,
        BakimDegerlendirmesi = 2,
        UretimKontrolu = 3,
        OnayBekliyor = 4,
        Onaylandi = 5,
        IptalEdildi = 6,
        GeriGonderildi = 7
    }
}
