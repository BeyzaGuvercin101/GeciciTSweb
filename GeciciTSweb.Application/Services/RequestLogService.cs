using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Services
{
    public class RequestLogService : IRequestLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RequestLogDto>> GetByRequestIdAsync(int requestId)
        {
            var logs = await _unitOfWork.RequestLogs.FindAsync(l => l.MaintenanceRequestId == requestId && !l.IsDeleted);
            return _mapper.Map<IEnumerable<RequestLogDto>>(logs);
        }

        public async Task<int> CreateAsync(CreateRequestLogDto dto)
        {
            var entity = _mapper.Map<RequestLog>(dto);
            entity.CreatedAt = DateTime.Now;
            
            await _unitOfWork.RequestLogs.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }
    }
}
