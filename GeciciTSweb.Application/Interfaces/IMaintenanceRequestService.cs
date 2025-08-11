using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces;

public interface IMaintenanceRequestService
{
    Task<int> CreateAsync(CreateMaintenanceRequestDto dto, string keycloakSub);
    Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto, string keycloakSub);
    Task<bool> PatchAsync(PatchMaintenanceRequestDto dto, string keycloakSub);
    Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync();
    Task<IEnumerable<MaintenanceRequestListDto>> GetByUserAsync(string keycloakSub);
    Task<MaintenanceRequestDto?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id, string keycloakSub); // soft delete
}
