using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UnitListDto>> GetAllAsync()
        {
            var result = (
                     from units in await _unitOfWork.Units.GetAllAsync()
                     join consoles in await _unitOfWork.Consoles.GetAllAsync()
                     on units.ConsoleId equals consoles.Id into consolesJoin
                     from consoles in consolesJoin.DefaultIfEmpty()
                     join company in await _unitOfWork.Companies.GetAllAsync()
                     on consoles != null ? consoles.CompanyId : (int?)null equals company.Id into companiesJoin
                     from company in companiesJoin.DefaultIfEmpty()
                     where !units.IsDeleted
                     select new UnitListDto
                     {
                         Id = units.Id,
                         Name = units.Name,
                         ConsoleId = consoles?.Id,
                         ConsoleName = consoles?.Name,
                         CompanyName = company?.Name,
                         CompanyId = company?.Id
                     }
                 ).ToList();
            return result;
        }

        public async Task<List<UnitListDto>> GetByConsoleIdAsync(int consoleId)
        {
            var result = (
                    from units in await _unitOfWork.Units.GetAllAsync()
                    join consoles in await _unitOfWork.Consoles.GetAllAsync() 
                    on units.ConsoleId equals consoles.Id into consolesJoin
                    from consoles in consolesJoin.DefaultIfEmpty()
                    join company in await _unitOfWork.Companies.GetAllAsync()
                    on consoles != null ? consoles.CompanyId : (int?)null equals company.Id into companiesJoin
                    from company in companiesJoin.DefaultIfEmpty()
                    where !units.IsDeleted && units.ConsoleId == consoleId
                    select new UnitListDto
                    {
                        Id = units.Id,
                        Name = units.Name,
                        ConsoleId = consoles.Id,
                        ConsoleName = consoles.Name,
                        CompanyName = company.Name,
                        CompanyId = company?.Id
                    }
                ).ToList();
            return result;
        }

        public async Task<UnitListDto> GetByIdAsync(int id)
        {
            var unit = await _unitOfWork.Units.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            return unit == null
                ? throw new Exception("Unit not found")
                : _mapper.Map<UnitListDto>(unit);
        }

        public async Task<UnitListDto> CreateAsync(CreateUnitDto dto)
        {
            var unit = _mapper.Map<Unit>(dto);
            await _unitOfWork.Units.AddAsync(unit);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UnitListDto>(unit);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var unit = await _unitOfWork.Units.GetByIdAsync(id);
            if (unit == null || unit.IsDeleted) return false;
            unit.IsDeleted = true;
            _unitOfWork.Units.Update(unit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

