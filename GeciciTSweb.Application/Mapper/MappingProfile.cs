using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Domain.Enums; // Enum için gerekli
using GeciciTSweb.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Company
            CreateMap<Companies, CompaniesListDto>().ReverseMap();
            CreateMap<Companies, CreateCompaniesDto>().ReverseMap();

            // Console
            CreateMap<Infrastructure.Entities.Console, ConsoleListDto>().ReverseMap();
            CreateMap<Infrastructure.Entities.Console, CreateConsoleDto>().ReverseMap();
            
            CreateMap<CreateRiskAssessmentDto, RiskAssessment>();
            CreateMap<UpdateRiskAssessmentDto, RiskAssessment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Unit
            CreateMap<Unit, UnitListDto>()
                .ForMember(dest => dest.ConsoleName, opt => opt.MapFrom(src => src.Console != null ? src.Console.Name : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Console != null && src.Console.Company != null ? src.Console.Company.Name : string.Empty));
            CreateMap<Unit, CreateUnitDto>().ReverseMap();

            // TemporaryMaintenanceType
            CreateMap<TemporaryMaintenanceType, TemporaryMaintenanceTypeListDto>().ReverseMap();
            CreateMap<TemporaryMaintenanceType, CreateTemporaryMaintenanceTypeDto>().ReverseMap();
            
            CreateMap<CreateMaintenanceRequestDto, MaintenanceRequest>();
            
            CreateMap<MaintenanceRequest, MaintenanceRequestDetailDto>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name))
                .ForMember(dest => dest.ConsoleName, opt => opt.MapFrom(src => src.Unit.Console != null ? src.Unit.Console.Name : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Unit.Console != null && src.Unit.Console.Company != null ? src.Unit.Console.Company.Name : string.Empty))
                .ForMember(dest => dest.TempMaintenanceTypeName, opt => opt.MapFrom(src => src.TempMaintenanceType.Name));
            
            CreateMap<MaintenanceRequest, MaintenanceRequestListDto>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name))
                .ForMember(dest => dest.ConsoleName, opt => opt.MapFrom(src => src.Unit.Console != null ? src.Unit.Console.Name : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Unit.Console != null && src.Unit.Console.Company != null ? src.Unit.Console.Company.Name : string.Empty))
                .ForMember(dest => dest.TempMaintenanceTypeName, opt => opt.MapFrom(src => src.TempMaintenanceType.Name));
            
            CreateMap<MaintenanceRequest, MaintenanceRequestDto>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name))
                .ForMember(dest => dest.ConsoleName, opt => opt.MapFrom(src => src.Unit.Console != null ? src.Unit.Console.Name : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Unit.Console != null && src.Unit.Console.Company != null ? src.Unit.Console.Company.Name : string.Empty))
                .ForMember(dest => dest.TempMaintenanceTypeName, opt => opt.MapFrom(src => src.TempMaintenanceType.Name));


            CreateMap<UpdateMaintenanceRequestDto, MaintenanceRequest>();

            CreateMap<MaintenanceRequest, UpdateMaintenanceRequestDto>();
            CreateMap<RiskAssessment, RiskAssessmentDto>().ReverseMap();


            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, RegisterResponse>().ReverseMap();
            
        }
    }
}
