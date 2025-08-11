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
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CompaniesListDto>> GetAllAsync()
        {
            var companies = await _unitOfWork.Companies.FindAsync(c => !c.IsDeleted);
            return _mapper.Map<List<CompaniesListDto>>(companies);
        }

        public async Task<CompaniesListDto> GetByIdAsync(int id)
        {
            var company = await _unitOfWork.Companies
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (company == null)
                throw new Exception("Şirket bulunamadı.");

            return _mapper.Map<CompaniesListDto>(company);
        }

        public async Task<int> CreateAsync(CreateCompaniesDto dto)
        {
            try
            {
                // Aynı isimde şirket var mı kontrol et
                var existingCompany = await _unitOfWork.Companies
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower() && !c.IsDeleted);
                
                if (existingCompany != null)
                {
                    throw new InvalidOperationException($"'{dto.Name}' isimli şirket zaten mevcut.");
                }

                var entity = _mapper.Map<Companies>(dto);
                await _unitOfWork.Companies.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Şirket oluşturulurken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            if (company == null || company.IsDeleted) return false;

            company.IsDeleted = true;
            _unitOfWork.Companies.Update(company);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
