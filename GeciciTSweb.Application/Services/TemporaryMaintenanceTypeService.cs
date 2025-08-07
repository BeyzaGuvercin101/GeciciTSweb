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
    public class TemporaryMaintenanceTypeService : ITemporaryMaintenanceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TemporaryMaintenanceTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TemporaryMaintenanceTypeListDto>> GetAllAsync()
        {
            var types = await _unitOfWork.TemporaryMaintenanceTypes.FindAsync(x => !x.IsDeleted);
            return _mapper.Map<List<TemporaryMaintenanceTypeListDto>>(types);
        }

        public async Task<TemporaryMaintenanceTypeListDto> GetByIdAsync(int id)
        {
            var type = await _unitOfWork.TemporaryMaintenanceTypes
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (type == null)
                throw new Exception("Geçici tamir tipi bulunamadı.");

            return _mapper.Map<TemporaryMaintenanceTypeListDto>(type);
        }

        public async Task<int> CreateAsync(CreateTemporaryMaintenanceTypeDto dto)
        {
            var entity = _mapper.Map<TemporaryMaintenanceType>(dto);
            await _unitOfWork.TemporaryMaintenanceTypes.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }
    }
}
