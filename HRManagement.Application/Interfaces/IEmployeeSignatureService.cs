using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeSignatureService
    {
        Task<EmployeeSignatureDto> Create(CreateEmployeeSignatureDto createDto, Stream? imageStream, string? fileName);
        Task<EmployeeSignatureDto?> GetById(long id);
        Task<EmployeeSignatureDto?> GetByEmployeeId(long employeeId);
        Task<PagedResult<EmployeeSignatureDto>> GetPaged(int pageNumber, int pageSize);
        Task<EmployeeSignatureDto> Update(long id, UpdateEmployeeSignatureDto updateDto);
        Task<EmployeeSignatureDto> UpdateSignatureImageAsync(long id, Stream imageStream, string fileName);
        Task Delete(long id);
    }
}
