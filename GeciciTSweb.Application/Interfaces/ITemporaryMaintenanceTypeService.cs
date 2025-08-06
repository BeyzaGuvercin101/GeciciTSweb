using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeciciTSweb.Application.DTOs;


namespace GeciciTSweb.Application.Interfaces
{
    public interface ITemporaryMaintenanceTypeService
    {
        Task<List<TemporaryMaintenanceTypeListDto>> GetAllAsync();
        Task<TemporaryMaintenanceTypeListDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateTemporaryMaintenanceTypeDto dto);
    }
}
