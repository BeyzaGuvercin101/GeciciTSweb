using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IUnitService
    {
        Task<List<UnitListDto>> GetAllAsync();
        Task<List<UnitListDto>> GetByConsoleIdAsync(int consoleId);
        Task<UnitListDto> GetByIdAsync(int id);
        Task<UnitListDto> CreateAsync(CreateUnitDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
