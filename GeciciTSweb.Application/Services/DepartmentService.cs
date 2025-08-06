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
    public class DepartmentService : IDepartmentService
    {
        private readonly GeciciTSwebDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(GeciciTSwebDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DepartmentListDto>> GetAllAsync()
        {
            var departments = await _context.Departments
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return _mapper.Map<List<DepartmentListDto>>(departments);
        }

        public async Task<DepartmentListDto> CreateAsync(CreateDepartmentDto dto)
        {
            var entity = _mapper.Map<Department>(dto);
            _context.Departments.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<DepartmentListDto>(entity);
        }
    }
}
