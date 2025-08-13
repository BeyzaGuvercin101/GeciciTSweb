using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces;

public interface IMaintenanceRequestService
{
    Task<int> CreateAsync(CreateMaintenanceRequestDto dto, string username);
    Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto, string username);
    Task<bool> PatchAsync(PatchMaintenanceRequestDto dto, string username);
    Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync();
    Task<IEnumerable<MaintenanceRequestListDto>> GetByUserAsync(string username);
    Task<MaintenanceRequestDto?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id, string username); // soft delete
}
