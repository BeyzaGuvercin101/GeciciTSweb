using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Console = GeciciTSweb.Infrastructure.Entities.Console;

namespace GeciciTSweb.Application.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConsoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ConsoleListDto>> GetAllAsync()
        {

            var list = (from console in await _unitOfWork.Consoles.GetAllAsync()
                        join company in await _unitOfWork.Companies.GetAllAsync()
                        on console.CompanyId equals company.Id into companyJoin
                        from company in companyJoin.DefaultIfEmpty()
                        select new ConsoleListDto
                        {
                            Id = console.Id,
                            Name = console.Name,
                            CompanyId = company?.Id,
                            CompanyName = company?.Name
                        }


                        ).ToList();


            return _mapper.Map<List<ConsoleListDto>>(list);
        }

        public async Task<List<ConsoleListDto>> GetByCompanyIdAsync(int companyId)
        {
            var consoles = await _unitOfWork.Consoles.FindAsync(c => !c.IsDeleted && c.CompanyId == companyId);
            return _mapper.Map<List<ConsoleListDto>>(consoles);
        }

        public async Task<ConsoleListDto> GetByIdAsync(int id)
        {
            var console = await _unitOfWork.Consoles.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (console == null)
                throw new Exception("Konsol bulunamadı.");

            return _mapper.Map<ConsoleListDto>(console);
        }

        public async Task<int> CreateAsync(CreateConsoleDto dto)
        {
            var entity = _mapper.Map<Console>(dto);
            await _unitOfWork.Consoles.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }
    }
}
