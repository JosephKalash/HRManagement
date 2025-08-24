using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeContactService
    {
        Task<EmployeeContactDto?> GetById(Guid id);
        Task<EmployeeContactDto?> GetByEmployeeId(Guid employeeId);
        Task<IEnumerable<EmployeeContactDto>> GetAll();
        Task<EmployeeContactDto> Create(CreateEmployeeContactDto createDto);
        Task<EmployeeContactDto> Update(Guid id, UpdateEmployeeContactDto updateDto);
        Task Delete(Guid id);
        Task<bool> Exists(Guid id);
        Task<PagedResult<EmployeeContactDto>> GetPaged(int pageNumber, int pageSize);
    }
}