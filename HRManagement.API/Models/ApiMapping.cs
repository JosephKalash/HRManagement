using AutoMapper;
using HRManagement.API.Models;
using HRManagement.Application.DTOs;

namespace HRManagement.API.Models
{
    /// <summary>
    /// AutoMapper profile for mapping API request models to application DTOs.
    /// </summary>
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            // API Profile Request to Application DTO
            CreateMap<CreateEmployeeProfileRequest, CreateEmployeeProfileDto>();
            CreateMap<UpdateEmployeeProfileRequest, UpdateEmployeeProfileDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // API Signature Request to Application DTO
            CreateMap<CreateEmployeeSignatureRequest, CreateEmployeeSignatureDto>();
            CreateMap<UpdateEmployeeSignatureRequest, UpdateEmployeeSignatureDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}