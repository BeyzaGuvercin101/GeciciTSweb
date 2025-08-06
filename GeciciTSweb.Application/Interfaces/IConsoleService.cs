using GeciciTSweb.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IConsoleService
    {
        Task<List<ConsoleListDto>> GetAllAsync();
        Task<ConsoleListDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateConsoleDto dto);
    }
}
