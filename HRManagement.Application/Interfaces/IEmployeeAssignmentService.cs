using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeAssignmentService
    {
        Task<EmployeeAssignmentDto?> GetById(long id);
        Task<IEnumerable<EmployeeAssignmentDto>> GetAll();
        Task<IEnumerable<EmployeeAssignmentDto>> GetByEmployeeId(long employeeId);
        Task<EmployeeAssignmentDto?> GetActiveByEmployeeId(long employeeId);
        Task<EmployeeAssignmentDto> Create(CreateEmployeeAssignmentDto createDto);
        Task<EmployeeAssignmentDto> Update(long id, UpdateEmployeeAssignmentDto updateDto);
        Task<List<EmployeeAssignmentDto>> GetByUnitId(long unitId);
        Task Delete(long id);
        Task<bool> Exists(long id);
        Task<PagedResult<EmployeeAssignmentDto>> GetPaged(int pageNumber, int pageSize);
    }
}