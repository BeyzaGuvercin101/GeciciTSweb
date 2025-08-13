using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace GeciciTSweb.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
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