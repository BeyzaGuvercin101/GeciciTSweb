using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Data;
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
        private readonly GeciciTSwebDbContext _context;
        private readonly IMapper _mapper;

        public UnitService(GeciciTSwebDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UnitListDto>> GetAllAsync()
        {
            var units = await _context.Units
                .Where(u => !u.IsDeleted)
                .ToListAsync();
            return _mapper.Map<List<UnitListDto>>(units);
        }

        public async Task<UnitListDto> GetByIdAsync(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            return unit == null || unit.IsDeleted
                ? throw new Exception("Unit not found")
                : _mapper.Map<UnitListDto>(unit);
        }

        public async Task<UnitListDto> CreateAsync(CreateUnitDto dto)
        {
            var unit = _mapper.Map<Unit>(dto);
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            return _mapper.Map<UnitListDto>(unit);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null || unit.IsDeleted) return false;

            unit.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

