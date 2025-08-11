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

        public async Task<int> CreateAsync(CreateRequestLogDto dto, string keycloakSub)
        {
            if (string.IsNullOrWhiteSpace(keycloakSub))
                throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

            // Keycloak Sub'dan User'ı bul veya oluştur
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
            if (user == null)
            {
                user = new User
                {
                    KeycloakSub = keycloakSub,
                    IsDeleted = false
                };
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }

            var entity = _mapper.Map<RequestLog>(dto);
            entity.AuthorUserId = user.Id;
            entity.CreatedAt = DateTime.UtcNow;
            
            await _unitOfWork.RequestLogs.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<RequestLogDto>> GetByUserAsync(string keycloakSub)
        {
            if (string.IsNullOrWhiteSpace(keycloakSub))
                throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
            if (user == null) return new List<RequestLogDto>();

            var logs = await _unitOfWork.RequestLogs.FindAsync(l => l.AuthorUserId == user.Id && !l.IsDeleted);
            return _mapper.Map<IEnumerable<RequestLogDto>>(logs);
        }

        public async Task<bool> SoftDeleteAsync(int id, string keycloakSub)
        {
            if (string.IsNullOrWhiteSpace(keycloakSub))
                throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

            var requestLog = await _unitOfWork.RequestLogs.GetByIdAsync(id);
            if (requestLog == null || requestLog.IsDeleted) return false;

            // Kullanıcının kendisine ait log'u silme yetkisi kontrolü
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
            if (user == null || requestLog.AuthorUserId != user.Id)
                throw new UnauthorizedAccessException("Bu kaydı silme yetkiniz yok.");

            requestLog.IsDeleted = true;
            _unitOfWork.RequestLogs.Update(requestLog);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
