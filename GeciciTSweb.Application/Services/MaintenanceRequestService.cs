using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Helpers;
using GeciciTSweb.Application.Interfaces;
using AutoMapper;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

public class MaintenanceRequestService : IMaintenanceRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MaintenanceRequestService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(CreateMaintenanceRequestDto dto)
    {
        var entity = _mapper.Map<MaintenanceRequest>(dto);
        entity.CreatedAt = DateTime.Now;
        entity.Status = MaintenanceWorkflowStatus.YeniTalep.ToString();
        entity.IsClosed = false;

        await _unitOfWork.MaintenanceRequests.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto)
    {
        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(dto.Id);
        if (entity == null || entity.IsDeleted) return false;

        entity.Temperature = dto.Temperature;
        entity.Pressure = dto.Pressure;
        entity.Fluid = dto.Fluid;
        entity.Status = dto.Status.ToString();
        entity.IsClosed = MaintenanceWorkflowHelper.IsClosed(dto.Status);
        entity.UpdatedAt = DateTime.Now;

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
    public async Task<MaintenanceRequestDto?> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.MaintenanceRequests.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        return entity == null ? null : _mapper.Map<MaintenanceRequestDto>(entity);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.MaintenanceRequests.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted) return false;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.Now;

        _unitOfWork.MaintenanceRequests.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.MaintenanceRequests.FindAsync(x => !x.IsDeleted);
        return _mapper.Map<IEnumerable<MaintenanceRequestListDto>>(entities);
    }

}

