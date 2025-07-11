using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(string department);
        Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
        Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
} 