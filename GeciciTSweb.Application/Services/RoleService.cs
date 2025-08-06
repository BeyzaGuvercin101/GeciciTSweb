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
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly GeciciTSwebDbContext _context;

        public RoleService(IMapper mapper, GeciciTSwebDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<RoleListDto>> GetAllAsync()
        {
            var roles = await _context.Roles.Where(x => !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<RoleListDto>>(roles);
        }

        public async Task<RoleListDto> GetByIdAsync(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (role == null)
                throw new Exception("Rol bulunamadı.");

            return _mapper.Map<RoleListDto>(role);
        }

        public async Task<int> CreateAsync(CreateRoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }
    }
}
