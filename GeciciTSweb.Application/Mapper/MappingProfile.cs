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
            CreateMap<Company, CompanyListDto>().ReverseMap();
            CreateMap<Company, CreateCompanyDto>().ReverseMap();

            // Console
            CreateMap<Infrastructure.Entities.Console, ConsoleListDto>().ReverseMap();
            CreateMap<Infrastructure.Entities.Console, CreateConsoleDto>().ReverseMap();

            // Department
            CreateMap<Department, DepartmentListDto>().ReverseMap();
            CreateMap<Department, CreateDepartmentDto>().ReverseMap();

            // Role
            CreateMap<Role, RoleListDto>().ReverseMap();
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            // User
            CreateMap<User, UserListDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();

            // Unit
            CreateMap<Unit, UnitListDto>().ReverseMap();
            CreateMap<Unit, CreateUnitDto>().ReverseMap();

            // TemporaryMaintenanceType
            CreateMap<TemporaryMaintenanceType, TemporaryMaintenanceTypeListDto>().ReverseMap();
            CreateMap<TemporaryMaintenanceType, CreateTemporaryMaintenanceTypeDto>().ReverseMap();


            // MaintenanceRequest
            CreateMap<MaintenanceRequest, MaintenanceRequestListDto>()
                .ForMember(dest => dest.WorkflowStatus, opt => opt.MapFrom(src =>
                    Enum.Parse<MaintenanceWorkflowStatus>(src.Status)))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.WorkflowStatus.ToString()));

            CreateMap<MaintenanceRequest, MaintenanceRequestDetailDto>().ReverseMap();
            CreateMap<MaintenanceRequest, CreateMaintenanceRequestDto>().ReverseMap();
            CreateMap<MaintenanceRequest, UpdateMaintenanceRequestDto>().ReverseMap();

            // RequestLog
            CreateMap<RequestLog, RequestLogDto>().ReverseMap();
            CreateMap<RequestLog, CreateRequestLogDto>().ReverseMap();
        }
    }
}
