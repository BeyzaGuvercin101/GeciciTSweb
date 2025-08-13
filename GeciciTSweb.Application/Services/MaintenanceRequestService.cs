using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Helpers;
using GeciciTSweb.Application.Interfaces;
using AutoMapper;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using GeciciTSweb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GeciciTSweb.Application.Services;

public class MaintenanceRequestService : IMaintenanceRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly GeciciTSwebDbContext _context;

    public MaintenanceRequestService(IUnitOfWork unitOfWork, IMapper mapper, GeciciTSwebDbContext context)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
    }

    public async Task<int> CreateAsync(CreateMaintenanceRequestDto dto, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));

        // Username'den User'ı bul veya oluştur
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            // Kullanıcı yoksa oluştur
            user = new User
            {
                Username = username,
                IsDeleted = false
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(); // User'ı kaydet ki ID'sini alalım
        }

        var entity = _mapper.Map<MaintenanceRequest>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = MaintenanceWorkflowStatus.YeniTalep;
        entity.IsClosed = false;
        entity.CreatedByUserId = user.Id;

        await _unitOfWork.MaintenanceRequests.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));

        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(dto.Id);
        if (entity == null || entity.IsDeleted) return false;

        // Kullanıcının bu kaydı güncelleme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || entity.CreatedByUserId != user.Id)
            throw new UnauthorizedAccessException("Bu kaydı güncelleme yetkiniz yok.");

        entity.Temperature = dto.Temperature;
        entity.Pressure = dto.Pressure;
        entity.TempMaintenanceTypeId = dto.TempMaintenanceTypeId;
        entity.Fluid = dto.Fluid;
        entity.Status = dto.Status;
        entity.IsClosed = MaintenanceWorkflowHelper.IsClosed(dto.Status);
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PatchAsync(PatchMaintenanceRequestDto dto, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));

        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(dto.Id);
        if (entity == null || entity.IsDeleted) return false;

        // Kullanıcının bu kaydı güncelleme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
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
            entity.Temperature = dto.Temperature;
        
        if (dto.Pressure.HasValue)
            entity.Pressure = dto.Pressure;
        
        if (!string.IsNullOrEmpty(dto.Fluid))
            entity.Fluid = dto.Fluid;

        entity.UpdatedAt = DateTime.UtcNow;

       
        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<MaintenanceRequestDto?> GetByIdAsync(int id)
    {
        var entity = await _context.MaintenanceRequests
            .IgnoreQueryFilters()                 // Unit/Console/Company üzerindeki filtreleri kapat
        .Where(x => !x.IsDeleted)             // sadece MR için soft delete uygula
            .Include(x => x.Unit)
                .ThenInclude(u => u.Console)
                    .ThenInclude(c => c.Company)
            .Include(x => x.TempMaintenanceType)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        return entity == null ? null : _mapper.Map<MaintenanceRequestDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));

        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted) return false;

        // Kullanıcının kendisine ait olan kaydı silme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || entity.CreatedByUserId != user.Id)
            throw new UnauthorizedAccessException("Bu kaydı silme yetkiniz yok.");

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MaintenanceRequestListDto>> GetByUserAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));

        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return new List<MaintenanceRequestListDto>();

        var entities = await _context.MaintenanceRequests
            .IgnoreQueryFilters()                 // Unit/Console/Company üzerindeki filtreleri kapat
        .Where(x => !x.IsDeleted)             // sadece MR için soft delete uygula
            .Include(x => x.Unit)
                .ThenInclude(u => u.Console)
                    .ThenInclude(c => c.Company)
            .Include(x => x.TempMaintenanceType)
            .Where(x => x.CreatedByUserId == user.Id && !x.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MaintenanceRequestListDto>>(entities);
    }

    public async Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync()
    {
        var entities = await _context.MaintenanceRequests
            .IgnoreQueryFilters()                 // Unit/Console/Company üzerindeki filtreleri kapat
        .Where(x => !x.IsDeleted)             // sadece MR için soft delete uygula
            .Include(x => x.Unit)
                .ThenInclude(u => u.Console)
                    .ThenInclude(c => c.Company)
            .Include(x => x.TempMaintenanceType)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MaintenanceRequestListDto>>(entities);
    }
}