using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentListDto>> GetAllAsync();            
        Task<DepartmentListDto> CreateAsync(CreateDepartmentDto dto);   

    }
}
    