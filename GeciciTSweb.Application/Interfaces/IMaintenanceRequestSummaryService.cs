using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IMaintenanceRequestSummaryService
    {
        Task<MaintenanceRequestSummaryDto?> GetSummaryAsync(int maintenanceRequestId);
    }
}
