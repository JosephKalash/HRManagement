using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeAssignmentService(IEmployeeAssignmentRepository employeeAssignmentRepository, IMapper mapper) : IEmployeeAssignmentService
    {
        private readonly IEmployeeAssignmentRepository _employeeAssignmentRepository = employeeAssignmentRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeAssignmentDto?> GetByIdAsync(Guid id)
        {
            var assignment = await _employeeAssignmentRepository.GetByIdAsync(id);
            return assignment != null ? _mapper.Map<EmployeeAssignmentDto>(assignment) : null;
        }

        public async Task<IEnumerable<EmployeeAssignmentDto>> GetAllAsync()
        {
            var assignments = await _employeeAssignmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeAssignmentDto>>(assignments);
        }

        public async Task<IEnumerable<EmployeeAssignmentDto>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var assignments = await _employeeAssignmentRepository.GetByEmployeeIdAsync(employeeId);
            return _mapper.Map<IEnumerable<EmployeeAssignmentDto>>(assignments);
        }

        public async Task<EmployeeAssignmentDto?> GetActiveByEmployeeIdAsync(Guid employeeId)
        {
            var assignment = await _employeeAssignmentRepository.GetActiveByEmployeeIdAsync(employeeId);
            return assignment != null ? _mapper.Map<EmployeeAssignmentDto>(assignment) : null;
        }

        public async Task<EmployeeAssignmentDto> CreateAsync(CreateEmployeeAssignmentDto createDto)
        {
            var assignment = _mapper.Map<EmployeeAssignment>(createDto);
            var createdAssignment = await _employeeAssignmentRepository.AddAsync(assignment);
            return _mapper.Map<EmployeeAssignmentDto>(createdAssignment);
        }

        public async Task<EmployeeAssignmentDto> UpdateAsync(Guid id, UpdateEmployeeAssignmentDto updateDto)
        {
            var assignment = await _employeeAssignmentRepository.GetByIdAsync(id) ?? throw new ArgumentException("Employee assignment not found");
            _mapper.Map(updateDto, assignment);
            var updatedAssignment = await _employeeAssignmentRepository.UpdateAsync(assignment);
            return _mapper.Map<EmployeeAssignmentDto>(updatedAssignment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var assignment = await _employeeAssignmentRepository.GetByIdAsync(id) ?? throw new ArgumentException("Employee assignment not found");
            await _employeeAssignmentRepository.DeleteAsync(assignment);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _employeeAssignmentRepository.ActiveExistsAsync(id);
        }

        public async Task<PagedResult<EmployeeAssignmentDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeAssignmentRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<EmployeeAssignmentDto>>(paged.Items);
            return new PagedResult<EmployeeAssignmentDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<List<EmployeeAssignmentDto>> GetByUnitIdAsync(Guid unitId)
        {
            var assignments = await _employeeAssignmentRepository.GetByUnitIdAsync(unitId);
            return _mapper.Map<List<EmployeeAssignmentDto>>(assignments);
        }
    }
}