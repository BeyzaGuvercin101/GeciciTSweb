using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Data;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeciciTSweb.Application.Services
{
    public class TemporaryMaintenanceTypeService : ITemporaryMaintenanceTypeService
    {
        private readonly GeciciTSwebDbContext _context;
        private readonly IMapper _mapper;

        public TemporaryMaintenanceTypeService(GeciciTSwebDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TemporaryMaintenanceTypeListDto>> GetAllAsync()
        {
            var types = await _context.TemporaryMaintenanceTypes
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return _mapper.Map<List<TemporaryMaintenanceTypeListDto>>(types);
        }

        public async Task<TemporaryMaintenanceTypeListDto> GetByIdAsync(int id)
        {
            var type = await _context.TemporaryMaintenanceTypes
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (type == null)
                throw new Exception("Geçici tamir tipi bulunamadı.");

            return _mapper.Map<TemporaryMaintenanceTypeListDto>(type);
        }

        public async Task<int> CreateAsync(CreateTemporaryMaintenanceTypeDto dto)
        {
            var entity = _mapper.Map<TemporaryMaintenanceType>(dto);
            _context.TemporaryMaintenanceTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
