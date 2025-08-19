using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task<ShortEmployeeDto> GetByIdShort(Guid id);
        Task<List<ShortEmployeeDto>> GetByIdsShortAsync(List<Guid> ids);
        Task<ShortEmployeeDto> GetByMilitaryShort(int militaryNumber);
        Task<List<ShortEmployeeDto>> GetByMilitariesShortList(List<int> militaryNumbers);
        Task<EmployeeProfile?> GetProfileByEmployeeIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
        Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<string> UploadProfileImageAsync(Guid employeeId, Stream imageStream, string OriginalFileName);
        Task DeleteProfileImageAsync(Guid employeeId);
        Task<ComprehensiveEmployeeDto?> GetComprehensiveEmployeeByIdAsync(Guid id);
        Task<List<EmployeeJobSummaryDto>> GetEmployeeJobSummaryAsync(Guid employeeId);
        Task<CurrentEmployeeSummaryDto?> GetCurrentEmployeeSummaryAsync(Guid userId);
        Task<IEnumerable<EmployeeDto>> GetEmployeeByRoleIdAsync(Guid roleId);
    }
}