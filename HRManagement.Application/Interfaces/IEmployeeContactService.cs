using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeContactService
    {
        Task<EmployeeContactDto?> GetById(long id);
        Task<EmployeeContactDto?> GetByEmployeeId(long employeeId);
        Task<IEnumerable<EmployeeContactDto>> GetAll();
        Task<EmployeeContactDto> Create(CreateEmployeeContactDto createDto);
        Task<EmployeeContactDto> Update(long id, UpdateEmployeeContactDto updateDto);
        Task Delete(long id);
        Task<bool> Exists(long id);
        Task<PagedResult<EmployeeContactDto>> GetPaged(int pageNumber, int pageSize);
    }
}