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

        public async Task<EmployeeAssignmentDto?> GetById(long id)
        {
            var assignment = await _employeeAssignmentRepository.GetById(id);
            return assignment != null ? _mapper.Map<EmployeeAssignmentDto>(assignment) : null;
        }

        public async Task<IEnumerable<EmployeeAssignmentDto>> GetAll()
        {
            var assignments = await _employeeAssignmentRepository.GetAll();
            return _mapper.Map<IEnumerable<EmployeeAssignmentDto>>(assignments);
        }

        public async Task<IEnumerable<EmployeeAssignmentDto>> GetByEmployeeId(long employeeId)
        {
            var assignments = await _employeeAssignmentRepository.GetByEmployeeId(employeeId);
            return _mapper.Map<IEnumerable<EmployeeAssignmentDto>>(assignments);
        }

        public async Task<EmployeeAssignmentDto?> GetActiveByEmployeeId(long employeeId)
        {
            var assignment = await _employeeAssignmentRepository.GetActiveByEmployeeId(employeeId);
            return assignment != null ? _mapper.Map<EmployeeAssignmentDto>(assignment) : null;
        }

        public async Task<EmployeeAssignmentDto> Create(CreateEmployeeAssignmentDto createDto)
        {
            var assignment = _mapper.Map<EmployeeAssignment>(createDto);
            var createdAssignment = await _employeeAssignmentRepository.AddAsync(assignment);
            return _mapper.Map<EmployeeAssignmentDto>(createdAssignment);
        }

        public async Task<EmployeeAssignmentDto> Update(long id, UpdateEmployeeAssignmentDto updateDto)
        {
            var assignment = await _employeeAssignmentRepository.GetById(id) ?? throw new ArgumentException("Employee assignment not found");
            _mapper.Map(updateDto, assignment);
            var updatedAssignment = await _employeeAssignmentRepository.Update(assignment);
            return _mapper.Map<EmployeeAssignmentDto>(updatedAssignment);
        }

        public async Task Delete(long id)
        {
            var assignment = await _employeeAssignmentRepository.GetById(id) ?? throw new ArgumentException("Employee assignment not found");
            await _employeeAssignmentRepository.Delete(assignment);
        }

        public async Task<bool> Exists(long id)
        {
            return await _employeeAssignmentRepository.ActiveExists(id);
        }

        public async Task<PagedResult<EmployeeAssignmentDto>> GetPaged(int pageNumber, int pageSize)
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

        public async Task<List<EmployeeAssignmentDto>> GetByUnitId(long unitId)
        {
            var assignments = await _employeeAssignmentRepository.GetByUnitId(unitId);
            return _mapper.Map<List<EmployeeAssignmentDto>>(assignments);
        }
    }
}