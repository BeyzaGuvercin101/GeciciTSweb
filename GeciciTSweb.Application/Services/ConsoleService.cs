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
using Console = GeciciTSweb.Infrastructure.Entities.Console;

namespace GeciciTSweb.Application.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly GeciciTSwebDbContext _context;
        private readonly IMapper _mapper;

        public ConsoleService(GeciciTSwebDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ConsoleListDto>> GetAllAsync()
        {
            var consoles = await _context.Consoles
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return _mapper.Map<List<ConsoleListDto>>(consoles);
        }

        public async Task<ConsoleListDto> GetByIdAsync(int id)
        {
            var console = await _context.Consoles.FindAsync(id);

            if (console == null || console.IsDeleted)
                throw new Exception("Konsol bulunamadı.");

            return _mapper.Map<ConsoleListDto>(console);
        }

        public async Task<int> CreateAsync(CreateConsoleDto dto)
        {
            var entity = _mapper.Map<Console>(dto);
            _context.Consoles.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
