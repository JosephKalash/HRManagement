using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeAssignmentService
    {
        Task<EmployeeAssignmentDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeAssignmentDto>> GetAllAsync();
        Task<IEnumerable<EmployeeAssignmentDto>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeAssignmentDto?> GetActiveByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeAssignmentDto> CreateAsync(CreateEmployeeAssignmentDto createDto);
        Task<EmployeeAssignmentDto> UpdateAsync(Guid id, UpdateEmployeeAssignmentDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<EmployeeAssignmentDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
} 