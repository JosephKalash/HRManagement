using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeServiceInfoService
    {
        Task<EmployeeServiceInfoDto?> GetById(Guid id);
        Task<IEnumerable<EmployeeServiceInfoDto>> GetAll();
        Task<IEnumerable<EmployeeServiceInfoDto>> GetByEmployeeId(Guid employeeId);
        Task<EmployeeServiceInfoDto?> GetActiveByEmployeeId(Guid employeeId);
        Task<EmployeeServiceInfoDto> Create(CreateEmployeeServiceInfoDto createDto);
        Task<EmployeeServiceInfoDto> Update(Guid id, UpdateEmployeeServiceInfoDto updateDto);
        Task Delete(Guid id);
        Task<bool> ActiveExists(Guid id);
        Task<PagedResult<EmployeeServiceInfoDto>> GetPaged(int pageNumber, int pageSize);
        Task<PagedResult<EmployeeServiceInfoDto>> GetPagedByEmployeeId(Guid employeeId, int pageNumber, int pageSize);
    }
}