using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Data;
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
        private readonly GeciciTSwebDbContext _context;

        public CompanyService(IMapper mapper, GeciciTSwebDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CompaniesListDto>> GetAllAsync()
        {
            var companies = await _context.Companies.ToListAsync();
            return _mapper.Map<List<CompaniesListDto>>(companies);
        }

        public async Task<CompaniesListDto> GetByIdAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                throw new Exception("Şirket bulunamadı.");

            return _mapper.Map<CompaniesListDto>(company);
        }

        public async Task<int> CreateAsync(CreateCompaniesDto dto)
        {
            var entity = _mapper.Map<Companies>(dto);
            _context.Companies.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }

}
