using HRManagement.Application.DTOs;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(string department);
        Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
        Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto updateDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 