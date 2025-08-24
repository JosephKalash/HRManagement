using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetById(Guid id);
        Task<ShortEmployeeDto> GetByIdShort(Guid id);
        Task<List<ShortEmployeeDto>> GetByIdsShort(List<Guid> ids);
        Task<ShortEmployeeDto> GetByMilitaryShort(int militaryNumber);
        Task<List<ShortEmployeeDto>> GetByMilitariesShortList(List<int> militaryNumbers);
        Task<EmployeeProfile?> GetProfileByEmployeeId(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAll();
        Task<IEnumerable<EmployeeDto>> GetActiveEmployees();
        Task<IEnumerable<EmployeeDto>> SearchEmployees(string searchTerm);
        Task<EmployeeDto> Create(CreateEmployeeDto createDto);
        Task<EmployeeDto> Update(Guid id, UpdateEmployeeDto updateDto);
        Task Delete(Guid id);
        Task<bool> Exists(Guid id);
        Task<PagedResult<EmployeeDto>> GetPaged(int pageNumber, int pageSize);
        Task<string> UploadProfileImage(Guid employeeId, Stream imageStream, string OriginalFileName);
        Task DeleteProfileImage(Guid employeeId);
        Task<ComprehensiveEmployeeDto?> GetComprehensiveEmployeeById(Guid id);
        Task<List<EmployeeJobSummaryDto>> GetEmployeeJobSummary(Guid employeeId);
        Task<CurrentEmployeeSummaryDto?> GetCurrentEmployeeSummary(Guid userId);
        Task<List<EmployeeDto>> GetEmployeeByRoleId(Guid roleId);
        Task<List<Guid>> GetEmployeeRoleIds(Guid employeeId);
    }
}