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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<DepartmentListDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Departments.FindAsync(x => !x.IsDeleted);
            return _mapper.Map<List<DepartmentListDto>>(departments);
        }

        public async Task<DepartmentListDto> CreateAsync(CreateDepartmentDto dto)
        {
            var entity = _mapper.Map<Department>(dto);
            await _unitOfWork.Departments.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<DepartmentListDto>(entity);
        }
    }
}
