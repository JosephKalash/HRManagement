using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeProfileService
    {
        Task<EmployeeProfileDto?> GetById(Guid id);
        Task<EmployeeProfileDto?> GetByEmployeeId(Guid employeeId);
        Task<IEnumerable<EmployeeProfileDto>> GetAll();
        Task<EmployeeProfileDto> Create(CreateEmployeeProfileDto createDto, Stream? stream, string? fileName);
        Task<EmployeeProfileDto> Update(Guid id, UpdateEmployeeProfileDto updateDto, Stream? stream, string? fileName = null);
        Task Delete(Guid id);
        Task<bool> Exists(Guid id);
        Task<PagedResult<EmployeeProfileDto>> GetPaged(int pageNumber, int pageSize);
        Task UpdateEmployeeImage(Guid employeeId, string imagePath);
    }
}