using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using GeciciTSweb.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeciciTSweb.Application.Services;

public class IntegrityRiskAssessmentService : IIntegrityRiskAssessmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public IntegrityRiskAssessmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RiskAssessmentListDto> CreateAsync(CreateIntegrityRiskAssessmentDto createDto, string keycloakSub)
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

        // Aynı MaintenanceRequest için kullanıcı daha önce değerlendirme yapmış mı kontrol et
        var existingAssessment = await _unitOfWork.IntegrityRiskAssessments
            .FirstOrDefaultAsync(x => x.MaintenanceRequestId == createDto.MaintenanceRequestId && x.UserId == user.Id);

        if (existingAssessment != null)
            throw new InvalidOperationException("Bu bakım talebi için zaten risk değerlendirmesi yapılmış.");

        var entity = _mapper.Map<IntegrityRiskAssessment>(createDto);
        entity.UserId = user.Id;
        entity.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.IntegrityRiskAssessments.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<RiskAssessmentListDto>(entity);
        result.AssessmentType = "Integrity";
        return result;
    }

    public async Task<RiskAssessmentListDto?> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.IntegrityRiskAssessments.GetByIdAsync(id);

        if (entity == null) return null;

        var result = _mapper.Map<RiskAssessmentListDto>(entity);
        result.AssessmentType = "Integrity";
        return result;
    }

    public async Task<IEnumerable<RiskAssessmentListDto>> GetByMaintenanceRequestIdAsync(int maintenanceRequestId)
    {
        var entities = await _unitOfWork.IntegrityRiskAssessments
            .FindAsync(x => x.MaintenanceRequestId == maintenanceRequestId);

        var results = _mapper.Map<List<RiskAssessmentListDto>>(entities);
        results.ForEach(x => x.AssessmentType = "Integrity");
        return results;
    }

    public async Task<RiskAssessmentListDto?> UpdateAsync(int id, CreateIntegrityRiskAssessmentDto updateDto, string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var entity = await _unitOfWork.IntegrityRiskAssessments.GetByIdAsync(id);
        if (entity == null) return null;

        // Kullanıcının bu değerlendirmeyi güncelleme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null || entity.UserId != user.Id)
            throw new UnauthorizedAccessException("Bu değerlendirmeyi güncelleme yetkiniz yok.");

        _mapper.Map(updateDto, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.IntegrityRiskAssessments.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<RiskAssessmentListDto>(entity);
        result.AssessmentType = "Integrity";
        return result;
    }

    public async Task<bool> DeleteAsync(int id, string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var entity = await _unitOfWork.IntegrityRiskAssessments.GetByIdAsync(id);
        if (entity == null) return false;

        // Kullanıcının bu değerlendirmeyi silme yetkisi kontrolü
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null || entity.UserId != user.Id)
            throw new UnauthorizedAccessException("Bu değerlendirmeyi silme yetkiniz yok.");

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.IntegrityRiskAssessments.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<RiskAssessmentListDto>> GetByUserAsync(string keycloakSub)
    {
        if (string.IsNullOrWhiteSpace(keycloakSub))
            throw new ArgumentException("Keycloak Subject is required", nameof(keycloakSub));

        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.KeycloakSub == keycloakSub);
        if (user == null) return new List<RiskAssessmentListDto>();

        var entities = await _unitOfWork.IntegrityRiskAssessments
            .FindAsync(x => x.UserId == user.Id);

        var results = _mapper.Map<List<RiskAssessmentListDto>>(entities);
        results.ForEach(x => x.AssessmentType = "Integrity");
        return results;
    }
}
