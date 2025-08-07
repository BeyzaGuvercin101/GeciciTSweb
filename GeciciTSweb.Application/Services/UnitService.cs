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
            var units = await _unitOfWork.Units.FindAsync(u => !u.IsDeleted);
            return _mapper.Map<List<UnitListDto>>(units);
        }

        public async Task<List<UnitListDto>> GetByConsoleIdAsync(int consoleId)
        {
            var units = await _unitOfWork.Units.FindAsync(u => !u.IsDeleted && u.ConsoleId == consoleId);
            return _mapper.Map<List<UnitListDto>>(units);
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

