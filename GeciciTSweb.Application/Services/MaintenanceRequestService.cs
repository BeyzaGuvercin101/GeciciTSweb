using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Helpers;
using AutoMapper;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Data;
using GeciciTSweb.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeciciTSweb.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Entities;

public class MaintenanceRequestService : IMaintenanceRequestService
{
    private readonly GeciciTSwebDbContext _context;
    private readonly IMapper _mapper;

    public MaintenanceRequestService(GeciciTSwebDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(CreateMaintenanceRequestDto dto)
    {
        var entity = _mapper.Map<MaintenanceRequest>(dto);
        entity.CreatedAt = DateTime.Now;
        entity.Status = MaintenanceWorkflowStatus.YeniTalep.ToString();
        entity.IsClosed = false;

        _context.MaintenanceRequests.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdateMaintenanceRequestDto dto)
    {
        var entity = await _context.MaintenanceRequests.FindAsync(dto.Id);
        if (entity == null || entity.IsDeleted) return false;

        entity.Temperature = dto.Temperature;
        entity.Pressure = dto.Pressure;
        entity.Fluid = dto.Fluid;
        entity.Status = dto.Status.ToString();
        entity.IsClosed = MaintenanceWorkflowHelper.IsClosed(dto.Status);
        entity.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<MaintenanceRequestDto?> GetByIdAsync(int id)
    {
        var entity = await _context.MaintenanceRequests
            .Include(x => x.UnitId)
            .Include(x => x.TempMaintenanceType)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        return entity == null ? null : _mapper.Map<MaintenanceRequestDto>(entity);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.MaintenanceRequests.FindAsync(id);
        if (entity == null || entity.IsDeleted) return false;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<MaintenanceRequestListDto>> GetAllAsync()
    {
        var entities = await _context.MaintenanceRequests
            //.Include(x => x.Unit)
            .Include(x => x.TempMaintenanceType)
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // Geri dönüş tipini IEnumerable olarak değiştiriyoruz.
        // AutoMapper bu dönüşümü sorunsuz bir şekilde yapar.
        return _mapper.Map<IEnumerable<MaintenanceRequestListDto>>(entities);
    }





}

