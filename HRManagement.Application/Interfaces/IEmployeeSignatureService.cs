using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeSignatureService
    {
        Task<EmployeeSignatureDto> CreateAsync(CreateEmployeeSignatureDto createDto, Stream? imageStream, string? fileName);
        Task<EmployeeSignatureDto?> GetByIdAsync(Guid id);
        Task<EmployeeSignatureDto?> GetByEmployeeIdAsync(Guid employeeId);
        Task<PagedResult<EmployeeSignatureDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<EmployeeSignatureDto> UpdateAsync(Guid id, UpdateEmployeeSignatureDto updateDto);
        Task<EmployeeSignatureDto> UpdateSignatureImageAsync(Guid id, Stream imageStream, string fileName);
        Task DeleteAsync(Guid id);
    }
}
