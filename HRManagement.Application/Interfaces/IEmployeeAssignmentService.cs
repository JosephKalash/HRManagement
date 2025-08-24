using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeAssignmentService
    {
        Task<EmployeeAssignmentDto?> GetById(Guid id);
        Task<IEnumerable<EmployeeAssignmentDto>> GetAll();
        Task<IEnumerable<EmployeeAssignmentDto>> GetByEmployeeId(Guid employeeId);
        Task<EmployeeAssignmentDto?> GetActiveByEmployeeId(Guid employeeId);
        Task<EmployeeAssignmentDto> Create(CreateEmployeeAssignmentDto createDto);
        Task<EmployeeAssignmentDto> Update(Guid id, UpdateEmployeeAssignmentDto updateDto);
        Task<List<EmployeeAssignmentDto>> GetByUnitId(Guid unitId);
        Task Delete(Guid id);
        Task<bool> Exists(Guid id);
        Task<PagedResult<EmployeeAssignmentDto>> GetPaged(int pageNumber, int pageSize);
    }
}