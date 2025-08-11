using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Helpers;
using GeciciTSweb.Application.Interfaces;
using AutoMapper;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeciciTSweb.Application.Services;

public class MaintenanceRequestService : IMaintenanceRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MaintenanceRequestService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(CreateMaintenanceRequestDto dto, string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        // Keycloak Sub'dan User'ı bul veya oluştur
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null)
        {
            // Kullanıcı yoksa oluştur
            user = new User
            {
                KeycloakSub = keycloakSub,
                IsDeleted = false
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(); // User'ı kaydet ki ID'sini alalım
        }

        var entity = _mapper.Map<MaintenanceRequest>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = MaintenanceWorkflowStatus.YeniTalep.ToString();
        entity.IsClosed = false;
        entity.CreatedByUserId = user.Id;

        await _unitOfWork.MaintenanceRequests.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto, string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(dto.Id);
        if (entity == null || entity.IsDeleted) return false;

        // Kullanıcının bu kaydı güncelleme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null || entity.CreatedByUserId != user.Id)
            throw new UnauthorizedAccessException("Bu kaydı güncelleme yetkiniz yok.");

        entity.Temperature = dto.Temperature;
        entity.Pressure = dto.Pressure;
        entity.Fluid = dto.Fluid;
        entity.Status = dto.Status.ToString();
        entity.IsClosed = MaintenanceWorkflowHelper.IsClosed(dto.Status);
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PatchAsync(PatchMaintenanceRequestDto dto, string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(dto.Id);
        if (entity == null || entity.IsDeleted) return false;

        // Kullanıcının bu kaydı güncelleme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null || entity.CreatedByUserId != user.Id)
            throw new UnauthorizedAccessException("Bu kaydı güncelleme yetkiniz yok.");

        // Sadece null olmayan alanları güncelle (PATCH semantiği)
        if (!string.IsNullOrEmpty(dto.BildirimNumarasi))
            entity.BildirimNumarasi = dto.BildirimNumarasi;
        
        if (!string.IsNullOrEmpty(dto.EquipmentNumber))
            entity.EquipmentNumber = dto.EquipmentNumber;
        
        if (dto.TempMaintenanceTypeId.HasValue)
            entity.TempMaintenanceTypeId = dto.TempMaintenanceTypeId.Value;
        
        if (dto.Temperature.HasValue)
            entity.Temperature = dto.Temperature.Value;
        
        if (dto.Pressure.HasValue)
            entity.Pressure = dto.Pressure.Value;
        
        if (!string.IsNullOrEmpty(dto.Fluid))
            entity.Fluid = dto.Fluid;

        entity.UpdatedAt = DateTime.UtcNow;

        // Eğer güncelleme sebebi varsa RequestLog'a kaydet
        if (!string.IsNullOrEmpty(dto.UpdateReason))
        {
            var requestLog = new RequestLog
            {
                MaintenanceRequestId = entity.Id,
                AuthorUserId = user.Id,
                ActionType = "PartialUpdate",
                ActionNote = dto.UpdateReason,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.RequestLogs.AddAsync(requestLog);
        }

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<MaintenanceRequestDto?> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.MaintenanceRequests.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        return entity == null ? null : _mapper.Map<MaintenanceRequestDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted) return false;

        // Kullanıcının kendisine ait olan kaydı silme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null || entity.CreatedByUserId != user.Id)
            throw new UnauthorizedAccessException("Bu kaydı silme yetkiniz yok.");

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MaintenanceRequestListDto>> GetByUserAsync(string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null) return new List<MaintenanceRequestListDto>();

        var entities = await _unitOfWork.MaintenanceRequests.FindAsync(x => x.CreatedByUserId == user.Id && !x.IsDeleted);
        return _mapper.Map<IEnumerable<MaintenanceRequestListDto>>(entities);
    }

    public async Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.MaintenanceRequests.FindAsync(x => !x.IsDeleted);
        return _mapper.Map<IEnumerable<MaintenanceRequestListDto>>(entities);
    }
}