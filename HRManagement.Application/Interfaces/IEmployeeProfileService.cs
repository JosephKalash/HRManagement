using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeProfileService
    {
        Task<EmployeeProfileDto?> GetById(long id);
        Task<EmployeeProfileDto?> GetByEmployeeId(long employeeId);
        Task<IEnumerable<EmployeeProfileDto>> GetAll();
        Task<EmployeeProfileDto> Create(CreateEmployeeProfileDto createDto, Stream? stream, string? fileName);
        Task<EmployeeProfileDto> Update(long id, UpdateEmployeeProfileDto updateDto, Stream? stream, string? fileName = null);
        Task Delete(long id);
        Task<bool> Exists(long id);
        Task<PagedResult<EmployeeProfileDto>> GetPaged(int pageNumber, int pageSize);
        Task UpdateEmployeeImage(long employeeId, string imagePath);
    }
}