using GeciciTSweb.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleListDto>> GetAllAsync();
        Task<RoleListDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateRoleDto dto);
    }
}
