using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeProfileService
    {
        Task<EmployeeProfileDto?> GetByIdAsync(Guid id);
        Task<EmployeeProfileDto?> GetByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<EmployeeProfileDto>> GetAllAsync();
        Task<EmployeeProfileDto> CreateAsync(CreateEmployeeProfileDto createDto, Stream? stream, string? fileName);
        Task<EmployeeProfileDto> UpdateAsync(Guid id, UpdateEmployeeProfileDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<EmployeeProfileDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task UpdateEmployeeImageAsync(Guid employeeId, string imagePath);
    }
} 