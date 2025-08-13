using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ApprovedCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.Onaylandi),
                ReturnedCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.GeriGonderildi),
                RejectedCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.IptalEdildi),
                InProgressCount = requests.Count(x =>
                    x.Status == MaintenanceWorkflowStatus.YeniTalep ||
                    x.Status == MaintenanceWorkflowStatus.ButunlukDegerlendirmesi ||
                    x.Status == MaintenanceWorkflowStatus.BakimDegerlendirmesi ||
                    x.Status == MaintenanceWorkflowStatus.UretimKontrolu ||
                    x.Status == MaintenanceWorkflowStatus.OnayBekliyor)
            };

            return result;
        }

        public async Task<DashboardStatsDto> GetDetailedStatisticsAsync()
        {
            // Ana form verileri - silinmemiş formlar
            var requests = await _unitOfWork.MaintenanceRequests.FindAsync(x => !x.IsDeleted);
            
            // Risk assessment verileri
            var riskAssessments = await _unitOfWork.RiskAssessments.FindAsync(x => !x.IsDeleted);

            // Form durumlarına göre sayımlar (Status enum olarak tutuluyor)
            var onaylananCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.Onaylandi);
            var geriGonderilenCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.GeriGonderildi);
            var iptalEdilenCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.IptalEdildi);
            
            // Workflow aşamaları
            var yeniTalepCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.YeniTalep);
            var butunlukCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.ButunlukDegerlendirmesi);
            var bakimCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.BakimDegerlendirmesi);
            var uretimCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.UretimKontrolu);
            var onayBekliyorCount = requests.Count(x => x.Status == MaintenanceWorkflowStatus.OnayBekliyor);

            // Açık/Kapalı form sayıları
            var acikFormlar = requests.Count(x => !x.IsClosed);
            var kapaliFormlar = requests.Count(x => x.IsClosed);
            
            // Devam eden formlar = Açık olan formlar
            var devamEdenCount = acikFormlar;

            // Risk assessment işlem sayıları (DepartmentStatus bazında)
            var onaylanmisAssessments = riskAssessments.Count(x => x.DepartmentStatus == DepartmentStatus.Onaylandi);
            var geriGonderilmisAssessments = riskAssessments.Count(x => x.DepartmentStatus == DepartmentStatus.GeriGonderildi);
            var iptalAssessments = riskAssessments.Count(x => x.DepartmentStatus == DepartmentStatus.Iptal);
            var degerlendirmeAssessments = riskAssessments.Count(x => x.DepartmentStatus == DepartmentStatus.Degerlendirme);

            // Risk assessment tamamlanma durumu
            // Her form için 3 risk assessment olması gerekiyor (Integrity, Maintenance, Production)
            var totalExpectedAssessments = requests.Count() * 3;
            var completedAssessments = riskAssessments.Count();
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
                
                // Risk assessment işlem sayıları (RequestLog yerine RiskAssessment durumlarından)
                OnayIslemleri = onaylanmisAssessments,
                RedIslemleri = iptalAssessments,
                GeriGondermeIslemleri = geriGonderilmisAssessments,
                IptalIslemleri = iptalAssessments
            };

            return result;
        }

        private async Task<int> GetCompletedRiskAssessmentCount()
        {
            var riskAssessments = await _unitOfWork.RiskAssessments.FindAsync(x => !x.IsDeleted);
            return riskAssessments.Count();
        }
    }
}
