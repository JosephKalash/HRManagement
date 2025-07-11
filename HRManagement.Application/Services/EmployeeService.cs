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

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(string department)
        {
            var employees = await _employeeRepository.GetByDepartmentAsync(department);
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
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PhoneNumber = createDto.PhoneNumber,
                DateOfBirth = createDto.DateOfBirth,
                HireDate = createDto.HireDate,
                Position = createDto.Position,
                Department = createDto.Department,
                Salary = createDto.Salary,
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

            if (updateDto.FirstName != null)
                employee.FirstName = updateDto.FirstName;
            if (updateDto.LastName != null)
                employee.LastName = updateDto.LastName;
            if (updateDto.Email != null)
                employee.Email = updateDto.Email;
            if (updateDto.PhoneNumber != null)
                employee.PhoneNumber = updateDto.PhoneNumber;
            if (updateDto.DateOfBirth.HasValue)
                employee.DateOfBirth = updateDto.DateOfBirth.Value;
            if (updateDto.HireDate.HasValue)
                employee.HireDate = updateDto.HireDate.Value;
            if (updateDto.Position != null)
                employee.Position = updateDto.Position;
            if (updateDto.Department != null)
                employee.Department = updateDto.Department;
            if (updateDto.Salary.HasValue)
                employee.Salary = updateDto.Salary.Value;
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
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                Position = employee.Position,
                Department = employee.Department,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }
    }
}