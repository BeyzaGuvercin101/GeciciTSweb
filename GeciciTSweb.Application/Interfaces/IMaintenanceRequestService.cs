using GeciciTSweb.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeciciTSweb.Application.DTOs;

public interface IMaintenanceRequestService
{
    Task<int> CreateAsync(CreateMaintenanceRequestDto dto);
    Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto);
    Task<bool> PatchAsync(PatchMaintenanceRequestDto dto); // Kısmi güncelleme
    Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync();
    Task<MaintenanceRequestDto?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id); // soft delete
    
}
