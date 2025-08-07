using GeciciTSweb.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<List<CompaniesListDto>> GetAllAsync();
        Task<CompaniesListDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCompaniesDto dto);
    }
}
