using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeSignatureService
    {
        Task<EmployeeSignatureDto> Create(CreateEmployeeSignatureDto createDto, Stream? imageStream, string? fileName);
        Task<EmployeeSignatureDto?> GetById(Guid id);
        Task<EmployeeSignatureDto?> GetByEmployeeId(Guid employeeId);
        Task<PagedResult<EmployeeSignatureDto>> GetPaged(int pageNumber, int pageSize);
        Task<EmployeeSignatureDto> Update(Guid id, UpdateEmployeeSignatureDto updateDto);
        Task<EmployeeSignatureDto> UpdateSignatureImageAsync(Guid id, Stream imageStream, string fileName);
        Task Delete(Guid id);
    }
}
