using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GeciciTSweb.Domain.Enums.MaintenanceWorkflowStatus;

namespace GeciciTSweb.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardCardDto> GetCardStatisticsAsync()
        {
            var requests = await _unitOfWork.MaintenanceRequests.FindAsync(x => !x.IsDeleted);

            var result = new DashboardCardDto
            {
                //to sitring olmamalı.
                ApprovedCount = requests.Count(x => x.Status == Onaylandi.ToString().ToUpperInvariant()),
                ReturnedCount = requests.Count(x => x.Status == GeriGonderildi.ToString().ToUpperInvariant()),
                RejectedCount = requests.Count(x => x.Status == IptalEdildi.ToString().ToUpperInvariant()),
                InProgressCount = requests.Count(x =>
                    x.Status == YeniTalep.ToString().ToUpperInvariant() ||
                    x.Status == ButunlukDegerlendirmesi.ToString().ToUpperInvariant() ||
                    x.Status == BakimDegerlendirmesi.ToString().ToUpperInvariant() ||
                    x.Status == UretimKontrolu.ToString().ToUpperInvariant() ||
                    x.Status == OnayBekliyor.ToString().ToUpperInvariant())
            };

            return result;
        }

        public async Task<DashboardStatsDto> GetDetailedStatisticsAsync()
        {
            // Ana form verileri - silinmemiş formlar
            var requests = await _unitOfWork.MaintenanceRequests.FindAsync(x => !x.IsDeleted);
            
            // RequestLog verileri - akış işlemleri
            var requestLogs = await _unitOfWork.RequestLogs.FindAsync(x => !x.IsDeleted);

            // Form durumlarına göre sayımlar (Status string olarak tutuluyor)
            var onaylananCount = requests.Count(x => x.Status.ToUpper() == "ONAYLANDI");
            var geriGonderilenCount = requests.Count(x => x.Status.ToUpper() == "GERIGONDERILDI");
            var iptalEdilenCount = requests.Count(x => x.Status.ToUpper() == "IPTALEDILDI");
            
            // Workflow aşamaları
            var yeniTalepCount = requests.Count(x => x.Status.ToUpper() == "YENITTALEP");
            var butunlukCount = requests.Count(x => x.Status.ToUpper() == "BUTUNLUKDEGERLENDIRMESI");
            var bakimCount = requests.Count(x => x.Status.ToUpper() == "BAKIMDEGERLENDIRMESI");
            var uretimCount = requests.Count(x => x.Status.ToUpper() == "URETIMKONTROLU");
            var onayBekliyorCount = requests.Count(x => x.Status.ToUpper() == "ONAYBEKLIYOR");

            // Açık/Kapalı form sayıları
            var acikFormlar = requests.Count(x => !x.IsClosed);
            var kapaliFormlar = requests.Count(x => x.IsClosed);
            
            // Devam eden formlar = Açık olan formlar
            var devamEdenCount = acikFormlar;

            // RequestLog bazlı işlem sayıları
            var onayIslemleri = requestLogs.Count(x => x.ActionType.ToUpper() == "ONAY");
            var redIslemleri = requestLogs.Count(x => x.ActionType.ToUpper() == "RED");
            var geriGondermeIslemleri = requestLogs.Count(x => x.ActionType.ToUpper() == "GERI GÖNDER" || x.ActionType.ToUpper() == "GERIGÖNDER");
            var iptalIslemleri = requestLogs.Count(x => x.ActionType.ToUpper() == "İPTAL" || x.ActionType.ToUpper() == "IPTAL");

            // Risk assessment tamamlanma durumu
            // Her form için 3 risk assessment olması gerekiyor
            var totalExpectedAssessments = requests.Count() * 3;
            var completedAssessments = await GetCompletedRiskAssessmentCount();
            var bekleyenAssessments = totalExpectedAssessments - completedAssessments;

            var result = new DashboardStatsDto
            {
                // Ana kartlar için
                OnaylananFormlar = onaylananCount,
                GeriGonderilenFormlar = geriGonderilenCount,
                OnaylanmayanFormlar = iptalEdilenCount, // İptal edilen formlar
                DevamEdenFormlar = devamEdenCount, // Açık olan formlar
                IptalEdilenFormlar = iptalEdilenCount,
                
                // Workflow detayları
                YeniTalepCount = yeniTalepCount,
                ButunlukDegerlendirmesiCount = butunlukCount,
                BakimDegerlendirmesiCount = bakimCount,
                UretimKontroluCount = uretimCount,
                OnayBekliyorCount = onayBekliyorCount,
                
                // Form durumları
                ToplamForm = requests.Count(),
                KapaliFormlar = kapaliFormlar,
                AcikFormlar = acikFormlar,
                
                // Risk assessment durumu
                TamamlananRiskDegerlendirmeleri = completedAssessments,
                BekleyenRiskDegerlendirmeleri = bekleyenAssessments > 0 ? bekleyenAssessments : 0,
                
                // İşlem sayıları
                OnayIslemleri = onayIslemleri,
                RedIslemleri = redIslemleri,
                GeriGondermeIslemleri = geriGondermeIslemleri,
                IptalIslemleri = iptalIslemleri
            };

            return result;
        }

        private async Task<int> GetCompletedRiskAssessmentCount()
        {
            var integrityCount = (await _unitOfWork.Repository<Infrastructure.Entities.IntegrityRiskAssessment>()
                .FindAsync(x => !x.IsDeleted)).Count();
            var maintenanceCount = (await _unitOfWork.Repository<Infrastructure.Entities.MaintenanceRiskAssessment>()
                .FindAsync(x => !x.IsDeleted)).Count();
            var productionCount = (await _unitOfWork.Repository<Infrastructure.Entities.ProductionRiskAssessment>()
                .FindAsync(x => !x.IsDeleted)).Count();
                
            return integrityCount + maintenanceCount + productionCount;
        }
    }
}
