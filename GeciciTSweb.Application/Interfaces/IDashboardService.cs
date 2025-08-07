using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeciciTSweb.Application.DTOs;


namespace GeciciTSweb.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardCardDto> GetCardStatisticsAsync();
    }
}
