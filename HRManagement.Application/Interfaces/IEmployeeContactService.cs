using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeContactService
    {
        Task<EmployeeContactDto?> GetByIdAsync(Guid id);
        Task<EmployeeContactDto?> GetByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<EmployeeContactDto>> GetAllAsync();
        Task<EmployeeContactDto> CreateAsync(CreateEmployeeContactDto createDto);
        Task<EmployeeContactDto> UpdateAsync(Guid id, UpdateEmployeeContactDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<EmployeeContactDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
} 