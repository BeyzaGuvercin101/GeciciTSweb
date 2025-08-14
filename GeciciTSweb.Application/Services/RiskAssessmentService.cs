using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Data;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeciciTSweb.Application.Services
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        private readonly GeciciTSwebDbContext _context;
        private readonly IMapper _mapper;

        public RiskAssessmentService(GeciciTSwebDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RiskAssessmentDto?> GetByMaintenanceRequestAndDepartmentAsync(int maintenanceRequestId, DepartmentCode departmentCode)
        {
            var riskAssessment = await _context.RiskAssessments
                .FirstOrDefaultAsync(r =>
                    r.MaintenanceRequestId == maintenanceRequestId &&
                    r.DepartmentCode == (int)departmentCode);

            return riskAssessment != null ? _mapper.Map<RiskAssessmentDto>(riskAssessment) : null;
        }

        public async Task<List<RiskAssessmentDto>> GetByMaintenanceRequestAsync(int maintenanceRequestId)
        {
            var riskAssessments = await _context.RiskAssessments
                .Where(r => r.MaintenanceRequestId == maintenanceRequestId && !r.IsDeleted)
                .ToListAsync();

            return _mapper.Map<List<RiskAssessmentDto>>(riskAssessments);
        }
        public async Task<List<RiskAssessmentDto>> GetAllAsync()
        {
            var riskAssessments = await _context.RiskAssessments
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<RiskAssessmentDto>>(riskAssessments);
        }



        public async Task<RiskAssessmentDto> CreateAsync(CreateRiskAssessmentDto dto)
        {
            // Check if already exists
            var existing = await _context.RiskAssessments
                .FirstOrDefaultAsync(r => r.MaintenanceRequestId == dto.MaintenanceRequestId && 
                                         r.DepartmentCode == dto.DepartmentCode);
            
            //if (existing != null)
            //{
            //    throw new InvalidOperationException($"Risk assessment already exists for MaintenanceRequest {dto.MaintenanceRequestId} and Department {dto.DepartmentCode}");
            //}

            var riskAssessment = _mapper.Map<RiskAssessment>(dto);
            riskAssessment.CreatedAt = DateTime.UtcNow;
            riskAssessment.DepartmentStatus = DepartmentStatus.Degerlendirme;
            
            // Calculate RPN values
            //riskAssessment.CalculateRPN();
            riskAssessment.CurrentRPN = riskAssessment.CurrentProbability * riskAssessment.CurrentImpact;
            riskAssessment.ResidualRPN = riskAssessment.ResidualProbability * riskAssessment.ResidualImpact;

            _context.RiskAssessments.Add(riskAssessment);
            await _context.SaveChangesAsync();

            // Update MaintenanceRequest status
            await UpdateMaintenanceRequestStatusAsync(dto.MaintenanceRequestId);

            return _mapper.Map<RiskAssessmentDto>(riskAssessment);
        }

        public async Task<RiskAssessmentDto> UpdateAsync(int id, UpdateRiskAssessmentDto dto)
        {
            var riskAssessment = await _context.RiskAssessments.FindAsync(id);
            if (riskAssessment == null)
            {
                throw new InvalidOperationException($"Risk assessment with ID {id} not found");
            }

            _mapper.Map(dto, riskAssessment);
            riskAssessment.UpdatedAt = DateTime.UtcNow;

            // Calculate RPN values
            //riskAssessment.CalculateRPN();
            riskAssessment.CurrentRPN = riskAssessment.CurrentProbability * riskAssessment.CurrentImpact;
            riskAssessment.ResidualRPN = riskAssessment.ResidualProbability * riskAssessment.ResidualImpact;


            await _context.SaveChangesAsync();

            // Update MaintenanceRequest status if department status changed
            await UpdateMaintenanceRequestStatusAsync(riskAssessment.MaintenanceRequestId);

            return _mapper.Map<RiskAssessmentDto>(riskAssessment);
        }

        public async Task<RiskAssessmentDto> ApproveAsync(int id, int approvedByUserId)
        {
            var riskAssessment = await _context.RiskAssessments.FindAsync(id);
            if (riskAssessment == null)
            {
                throw new InvalidOperationException($"Risk assessment with ID {id} not found");
            }

            riskAssessment.DepartmentStatus = DepartmentStatus.Onaylandi;
            riskAssessment.ApprovedByUserId = approvedByUserId;
            riskAssessment.ApprovedAt = DateTime.UtcNow;
            riskAssessment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Update MaintenanceRequest status
            await UpdateMaintenanceRequestStatusAsync(riskAssessment.MaintenanceRequestId);

            return _mapper.Map<RiskAssessmentDto>(riskAssessment);
        }

        public async Task<RiskAssessmentDto> ReturnAsync(int id, string reasonCode, string reasonText)
        {
            var riskAssessment = await _context.RiskAssessments.FindAsync(id);
            if (riskAssessment == null)
            {
                throw new InvalidOperationException($"Risk assessment with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(reasonText))
            {
                throw new ArgumentException("Return reason text is required");
            }

            riskAssessment.DepartmentStatus = DepartmentStatus.GeriGonderildi;
            riskAssessment.ReturnReasonCode = reasonCode;
            riskAssessment.ReturnReasonText = reasonText;
            riskAssessment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Update MaintenanceRequest status
            await UpdateMaintenanceRequestStatusAsync(riskAssessment.MaintenanceRequestId);

            return _mapper.Map<RiskAssessmentDto>(riskAssessment);
        }

        public async Task<RiskAssessmentDto> CancelAsync(int id, string reasonCode, string reasonText)
        {
            var riskAssessment = await _context.RiskAssessments.FindAsync(id);
            if (riskAssessment == null)
            {
                throw new InvalidOperationException($"Risk assessment with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(reasonText))
            {
                throw new ArgumentException("Cancel reason text is required");
            }

            riskAssessment.DepartmentStatus = DepartmentStatus.Iptal;
            riskAssessment.CancelReasonCode = reasonCode;
            riskAssessment.CancelReasonText = reasonText;
            riskAssessment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Update MaintenanceRequest status
            await UpdateMaintenanceRequestStatusAsync(riskAssessment.MaintenanceRequestId);

            return _mapper.Map<RiskAssessmentDto>(riskAssessment);
        }

        public async Task DeleteAsync(int id)
        {
            var riskAssessment = await _context.RiskAssessments.FindAsync(id);
            if (riskAssessment == null)
            {
                throw new InvalidOperationException($"Risk assessment with ID {id} not found");
            }

            riskAssessment.IsDeleted = true;
            riskAssessment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Update MaintenanceRequest status
            await UpdateMaintenanceRequestStatusAsync(riskAssessment.MaintenanceRequestId);
        }
        //ilgili talebi bulur, yeni duruma göre deðiþmiþse ekleme yapar.
        public async Task UpdateMaintenanceRequestStatusAsync(int maintenanceRequestId)
        {
            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(maintenanceRequestId);
            if (maintenanceRequest == null) return;

            var newStatus = await CalculateMaintenanceRequestStatusAsync(maintenanceRequestId);
            
            if (maintenanceRequest.Status != newStatus)
            {
                maintenanceRequest.Status = newStatus;
                maintenanceRequest.IsClosed = newStatus == MaintenanceWorkflowStatus.Onaylandi;
                maintenanceRequest.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<MaintenanceWorkflowStatus> CalculateMaintenanceRequestStatusAsync(int maintenanceRequestId)
        {
            var riskAssessments = await _context.RiskAssessments
                .Where(r => r.MaintenanceRequestId == maintenanceRequestId)
                .ToListAsync();

            // Rule 1: Any department is Cancelled -> IptalEdildi
            if (riskAssessments.Any(r => r.DepartmentStatus == DepartmentStatus.Iptal))
            {
                return MaintenanceWorkflowStatus.IptalEdildi;
            }

            // Rule 2: Any department is Returned -> GeriGonderildi
            if (riskAssessments.Any(r => r.DepartmentStatus == DepartmentStatus.GeriGonderildi))
            {
                return MaintenanceWorkflowStatus.GeriGonderildi;
            }

            var integrityAssessment = riskAssessments.FirstOrDefault(r => r.DepartmentCode == (int)DepartmentCode.Integrity);
            var maintenanceAssessment = riskAssessments.FirstOrDefault(r => r.DepartmentCode == (int)DepartmentCode.Maintenance);
            var productionAssessment = riskAssessments.FirstOrDefault(r => r.DepartmentCode == (int)DepartmentCode.Production);

            // Rule 3: Integrity not approved -> ButunlukDegerlendirmesi
            if (integrityAssessment?.DepartmentStatus != DepartmentStatus.Onaylandi)
            {
                return MaintenanceWorkflowStatus.ButunlukDegerlendirmesi;
            }

            // Rule 4: Integrity approved AND Maintenance not approved -> BakimDegerlendirmesi
            if (integrityAssessment.DepartmentStatus == DepartmentStatus.Onaylandi &&
                maintenanceAssessment?.DepartmentStatus != DepartmentStatus.Onaylandi)
            {
                return MaintenanceWorkflowStatus.BakimDegerlendirmesi;
            }

            // Rule 5: Integrity & Maintenance approved, Production in evaluation -> UretimKontrolu
            if (integrityAssessment.DepartmentStatus == DepartmentStatus.Onaylandi &&
                maintenanceAssessment?.DepartmentStatus == DepartmentStatus.Onaylandi &&
                productionAssessment?.DepartmentStatus == DepartmentStatus.Degerlendirme)
            {
                return MaintenanceWorkflowStatus.UretimKontrolu;
            }

            // Rule 6: Production waiting for approval -> OnayBekliyor
            if (productionAssessment?.DepartmentStatus == DepartmentStatus.OnayBekliyor)
            {
                return MaintenanceWorkflowStatus.OnayBekliyor;
            }

            // Rule 7: All departments approved -> Onaylandi
            if (integrityAssessment?.DepartmentStatus == DepartmentStatus.Onaylandi &&
                maintenanceAssessment?.DepartmentStatus == DepartmentStatus.Onaylandi &&
                productionAssessment?.DepartmentStatus == DepartmentStatus.Onaylandi)
            {
                return MaintenanceWorkflowStatus.Onaylandi;
            }

            // Default case
            return MaintenanceWorkflowStatus.ButunlukDegerlendirmesi;
        }
    }
}
