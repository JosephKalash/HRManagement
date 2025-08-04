using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null ? MapToDto(employee) : null;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await _employeeRepository.GetActiveEmployeesAsync();
            return employees.Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm)
        {
            var employees = await _employeeRepository.SearchEmployeesAsync(searchTerm);
            return employees.Select(MapToDto);
        }

        public async Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            // Map entities to DTOs
            var dtoList = paged.Items.Select(MapToDto).ToList();
            return new PagedResult<EmployeeDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto)
        {
            var employee = new Employee
            {
                MilitaryNumber = createDto.MilitaryNumber,
                ArabicFirstName = createDto.ArabicFirstName,
                ArabicMiddleName = createDto.ArabicMiddleName,
                ArabicLastName = createDto.ArabicLastName,
                EnglishFirstName = createDto.EnglishFirstName,
                EnglishMiddleName = createDto.EnglishMiddleName,
                EnglishLastName = createDto.EnglishLastName,
                Gender = createDto.Gender,
                DateOfBirth = createDto.DateOfBirth,
                IdNumber = createDto.IdNumber,
                IsActive = true
            };

            var createdEmployee = await _employeeRepository.AddAsync(employee);
            return MapToDto(createdEmployee);
        }

        public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto updateDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            if (updateDto.MilitaryNumber.HasValue)
                employee.MilitaryNumber = updateDto.MilitaryNumber.Value;
            if (updateDto.ArabicFirstName != null)
                employee.ArabicFirstName = updateDto.ArabicFirstName;
            if (updateDto.ArabicMiddleName != null)
                employee.ArabicMiddleName = updateDto.ArabicMiddleName;
            if (updateDto.ArabicLastName != null)
                employee.ArabicLastName = updateDto.ArabicLastName;
            if (updateDto.EnglishFirstName != null)
                employee.EnglishFirstName = updateDto.EnglishFirstName;
            if (updateDto.EnglishMiddleName != null)
                employee.EnglishMiddleName = updateDto.EnglishMiddleName;
            if (updateDto.EnglishLastName != null)
                employee.EnglishLastName = updateDto.EnglishLastName;
            if (updateDto.Gender.HasValue)
                employee.Gender = updateDto.Gender.Value;
            if (updateDto.DateOfBirth.HasValue)
                employee.DateOfBirth = updateDto.DateOfBirth.Value;
            if (updateDto.IdNumber != null)
                employee.IdNumber = updateDto.IdNumber;
            if (updateDto.IsActive.HasValue)
                employee.IsActive = updateDto.IsActive.Value;

            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
            return MapToDto(updatedEmployee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _employeeRepository.ExistsAsync(id);
        }

        private static EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                MilitaryNumber = employee.MilitaryNumber,
                ArabicFirstName = employee.ArabicFirstName,
                ArabicMiddleName = employee.ArabicMiddleName,
                ArabicLastName = employee.ArabicLastName,
                EnglishFirstName = employee.EnglishFirstName,
                EnglishMiddleName = employee.EnglishMiddleName,
                EnglishLastName = employee.EnglishLastName,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                IdNumber = employee.IdNumber,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }
    }
}