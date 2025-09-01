using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetById(long id);
        Task<ShortEmployeeDto> GetByIdShort(long id);
        Task<List<ShortEmployeeDto>> GetByIdsShort(List<Guid> guids); // external
        Task<ShortEmployeeDto> GetByMilitaryShort(int militaryNumber);
        Task<List<ShortEmployeeDto>> GetByMilitariesShortList(List<int> militaryNumbers);
        Task<EmployeeProfile?> GetProfileByEmployeeId(long id);
        Task<IEnumerable<EmployeeDto>> GetAll();
        Task<IEnumerable<EmployeeDto>> GetActiveEmployees();
        Task<IEnumerable<EmployeeDto>> SearchEmployees(string searchTerm);
        Task<EmployeeDto> Create(CreateEmployeeDto createDto);
        Task<EmployeeDto> Update(long id, UpdateEmployeeDto updateDto);
        Task Delete(long id);
        Task<bool> Exists(long id);
        Task<PagedResult<EmployeeDto>> GetPaged(int pageNumber, int pageSize);
        Task<string> UploadProfileImage(long employeeId, Stream imageStream, string OriginalFileName);
        Task DeleteProfileImage(long employeeId);
        Task<ComprehensiveEmployeeDto?> GetComprehensiveEmployeeById(long id);
        Task<List<EmployeeJobSummaryDto>> GetEmployeeJobSummary(long employeeId);
        Task<EmployeeJobSummaryDto> GetEmployeeCurrentWorkingJob(long employeeId);
        Task<CurrentEmployeeSummaryDto?> GetCurrentEmployeeSummary(long userId);
        Task<List<EmployeeDto>> GetEmployeeByRoleId(long roleId);
        Task<List<Guid>> GetEmployeeRoleGuids(Guid employeeId); // external
    }
}