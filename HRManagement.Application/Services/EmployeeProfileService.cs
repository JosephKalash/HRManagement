using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeProfileService(IEmployeeProfileRepository employeeProfileRepository, IMapper mapper) : IEmployeeProfileService
    {
        private readonly IEmployeeProfileRepository _employeeProfileRepository = employeeProfileRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeProfileDto?> GetByIdAsync(Guid id)
        {
            var profile = await _employeeProfileRepository.GetByIdAsync(id);
            return profile != null ? _mapper.Map<EmployeeProfileDto>(profile) : null;
        }

        public async Task<EmployeeProfileDto?> GetByEmployeeIdAsync(Guid employeeId)
        {
            var profile = await _employeeProfileRepository.GetByEmployeeIdAsync(employeeId);
            return profile != null ? _mapper.Map<EmployeeProfileDto>(profile) : null;
        }

        public async Task<IEnumerable<EmployeeProfileDto>> GetAllAsync()
        {
            var profiles = await _employeeProfileRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeProfileDto>>(profiles);
        }

        public async Task<EmployeeProfileDto> CreateAsync(CreateEmployeeProfileDto createDto)
        {
            var profile = _mapper.Map<EmployeeProfile>(createDto);
            var createdProfile = await _employeeProfileRepository.AddAsync(profile);
            return _mapper.Map<EmployeeProfileDto>(createdProfile);
        }

        public async Task<EmployeeProfileDto> UpdateAsync(Guid id, UpdateEmployeeProfileDto updateDto)
        {
            var profile = await _employeeProfileRepository.GetByIdAsync(id) ?? throw new ArgumentException("Employee profile not found");
            _mapper.Map(updateDto, profile);
            var updatedProfile = await _employeeProfileRepository.UpdateAsync(profile);
            return _mapper.Map<EmployeeProfileDto>(updatedProfile);
        }

        public async Task DeleteAsync(Guid id)
        {
            var profile = await _employeeProfileRepository.GetByIdAsync(id) ?? throw new ArgumentException("Employee profile not found");
            await _employeeProfileRepository.DeleteAsync(profile);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _employeeProfileRepository.ExistsAsync(id);
        }

        public async Task<PagedResult<EmployeeProfileDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeProfileRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<EmployeeProfileDto>>(paged.Items);
            return new PagedResult<EmployeeProfileDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task UpdateEmployeeImageAsync(Guid employeeId, string imagePath)
        {
            await _employeeProfileRepository.UpdateEmployeeImageAsync(employeeId, imagePath);
        }
    }
} 