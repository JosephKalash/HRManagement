using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeServiceInfoService
    {
        Task<EmployeeServiceInfoDto?> GetById(long id);
        Task<IEnumerable<EmployeeServiceInfoDto>> GetAll();
        Task<IEnumerable<EmployeeServiceInfoDto>> GetByEmployeeId(long employeeId);
        Task<EmployeeServiceInfoDto?> GetActiveByEmployeeId(long employeeId);
        Task<EmployeeServiceInfoDto> Create(CreateEmployeeServiceInfoDto createDto);
        Task<EmployeeServiceInfoDto> Update(long id, UpdateEmployeeServiceInfoDto updateDto);
        Task Delete(long id);
        Task<bool> ActiveExists(long id);
        Task<PagedResult<EmployeeServiceInfoDto>> GetPaged(int pageNumber, int pageSize);
        Task<PagedResult<EmployeeServiceInfoDto>> GetPagedByEmployeeId(long employeeId, int pageNumber, int pageSize);
    }
}