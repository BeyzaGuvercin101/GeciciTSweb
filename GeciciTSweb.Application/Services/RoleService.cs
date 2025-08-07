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

namespace GeciciTSweb.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoleListDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.FindAsync(x => !x.IsDeleted);
            return _mapper.Map<List<RoleListDto>>(roles);
        }

        public async Task<RoleListDto> GetByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (role == null)
                throw new Exception("Rol bulunamadı.");

            return _mapper.Map<RoleListDto>(role);
        }

        public async Task<int> CreateAsync(CreateRoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
            return role.Id;
        }
    }
}
