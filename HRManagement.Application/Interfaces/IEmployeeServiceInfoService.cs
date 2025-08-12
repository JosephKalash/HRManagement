using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeServiceInfoService
    {
        Task<EmployeeServiceInfoDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeServiceInfoDto>> GetAllAsync();
        Task<IEnumerable<EmployeeServiceInfoDto>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeServiceInfoDto?> GetActiveByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeServiceInfoDto> CreateAsync(CreateEmployeeServiceInfoDto createDto);
        Task<EmployeeServiceInfoDto> UpdateAsync(Guid id, UpdateEmployeeServiceInfoDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ActiveExistsAsync(Guid id);
        Task<PagedResult<EmployeeServiceInfoDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagedResult<EmployeeServiceInfoDto>> GetPagedByEmployeeIdAsync(Guid employeeId, int pageNumber, int pageSize);
    }
}