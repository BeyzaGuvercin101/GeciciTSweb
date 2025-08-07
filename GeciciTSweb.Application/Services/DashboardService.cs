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
    }
}
