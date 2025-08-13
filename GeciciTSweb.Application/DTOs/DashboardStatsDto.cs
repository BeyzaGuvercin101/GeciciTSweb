using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class DashboardStatsDto
    {
        // Ana form durumları (Dashboard kartları için)
        public int OnaylananFormlar { get; set; }      // Status = "ONAYLANDI"
        public int GeriGonderilenFormlar { get; set; }  // Status = "GERIGONDERILDI"
        public int OnaylanmayanFormlar { get; set; }    // İptal edilen formlar (Status = "IPTALEDILDI")
        public int DevamEdenFormlar { get; set; }       // Açık olan formlar (IsClosed = false)
        public int IptalEdilenFormlar { get; set; }     // Status = "IPTALEDILDI"
        
        // Form workflow aşamaları - akış sırasındaki detaylar
        public int YeniTalepCount { get; set; }                    // Status = "YENITTALEP"
        public int ButunlukDegerlendirmesiCount { get; set; }      // Status = "BUTUNLUKDEGERLENDIRMESI"
        public int BakimDegerlendirmesiCount { get; set; }         // Status = "BAKIMDEGERLENDIRMESI"
        public int UretimKontroluCount { get; set; }               // Status = "URETIMKONTROLU"
        public int OnayBekliyorCount { get; set; }                 // Status = "ONAYBEKLIYOR"
        
        // Toplam ve kapalı formlar
        public int ToplamForm { get; set; }           // Tüm formlar (IsDeleted = false)
        public int KapaliFormlar { get; set; }        // IsClosed = true
        public int AcikFormlar { get; set; }          // IsClosed = false
        
        // Risk assessment tamamlanma durumu
        public int TamamlananRiskDegerlendirmeleri { get; set; }
        public int BekleyenRiskDegerlendirmeleri { get; set; }
        
        // RiskAssessment bazlı istatistikler (akış işlemleri)
        public int OnayIslemleri { get; set; }        // ActionType = "Onay"
        public int RedIslemleri { get; set; }         // ActionType = "Red"
        public int GeriGondermeIslemleri { get; set; } // ActionType = "Geri Gönder"
        public int IptalIslemleri { get; set; }       // ActionType = "İptal"
    }
}
