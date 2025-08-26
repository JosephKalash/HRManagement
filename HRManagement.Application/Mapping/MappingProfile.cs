using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;

namespace HRManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ReverseMap();

            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Profile, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceInfos, opt => opt.Ignore())
                .ForMember(dest => dest.Assignments, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Profile, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceInfos, opt => opt.Ignore())
                .ForMember(dest => dest.Assignments, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // EmployeeProfile mappings
            CreateMap<EmployeeProfile, EmployeeProfileDto>()
                .ReverseMap();

            CreateMap<CreateEmployeeProfileDto, EmployeeProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore());

            CreateMap<UpdateEmployeeProfileDto, EmployeeProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // EmployeeContact mappings
            CreateMap<EmployeeContact, EmployeeContactDto>()
                .ReverseMap();

            CreateMap<CreateEmployeeContactDto, EmployeeContact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore());

            CreateMap<UpdateEmployeeContactDto, EmployeeContact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // EmployeeServiceInfo mappings
            CreateMap<EmployeeServiceInfo, EmployeeServiceInfoDto>()
                .ReverseMap();

            CreateMap<CreateEmployeeServiceInfoDto, EmployeeServiceInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore());

            CreateMap<UpdateEmployeeServiceInfoDto, EmployeeServiceInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // EmployeeAssignment mappings
            CreateMap<EmployeeAssignment, EmployeeAssignmentDto>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByUser == null ? null : src.CreatedByUser.ArabicFirstName + " " + src.CreatedByUser.ArabicLastName))
                .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedByUser == null ? null : src.UpdatedByUser.ArabicFirstName + " " + src.UpdatedByUser.ArabicLastName))
                .ReverseMap();

            CreateMap<CreateEmployeeAssignmentDto, EmployeeAssignment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUnit, opt => opt.Ignore())
                .ForMember(dest => dest.JobRole, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<UpdateEmployeeAssignmentDto, EmployeeAssignment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUnit, opt => opt.Ignore())
                .ForMember(dest => dest.JobRole, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // OrgUnit mappings
            CreateMap<OrgUnit, OrgUnitDto>()
                .ReverseMap();

            CreateMap<CreateOrgUnitDto, OrgUnit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeAssignments, opt => opt.Ignore());

            CreateMap<UpdateOrgUnitDto, OrgUnit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeAssignments, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // OrgUnitHierarchyDto mapping
            CreateMap<OrgUnit, OrgUnitHierarchyDto>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));

            // Role mappings
            CreateMap<Role, RoleDto>()
                // .ForMember(dest => dest.LevelNumber, opt => opt.MapFrom(src => (int)src.Level))
                .ReverseMap();

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeAssignments, opt => opt.Ignore());

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeAssignments, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // EmployeeJobSummaryDto mappings
            CreateMap<EmployeeServiceInfo, EmployeeJobSummaryDto>()
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.BelongingUnitId))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.BelongingUnit.Name))
                .ForMember(dest => dest.JobRoleName, opt => opt.MapFrom(src => src.JobRole.Name));

            CreateMap<EmployeeAssignment, EmployeeJobSummaryDto>()
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.AssignedUnitId))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.AssignedUnit.Name))
                .ForMember(dest => dest.JobRoleName, opt => opt.MapFrom(src => src.JobRole.Name))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.AssignmentDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); // Assuming assignments are always active for job details

            // EmployeeSignature mappings
            CreateMap<EmployeeSignature, EmployeeSignatureDto>()
                .ReverseMap();

            CreateMap<CreateEmployeeSignatureDto, EmployeeSignature>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalFileName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateEmployeeSignatureDto, EmployeeSignature>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalFileName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ComprehensiveEmployeeDto mapping
            CreateMap<Employee, ComprehensiveEmployeeDto>()
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contact))
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.Signature))
                .ForMember(dest => dest.ActiveServiceInfo, opt => opt.MapFrom(src => src.ServiceInfos.FirstOrDefault(si => si.IsActive)))
                .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));
        }
    }
}