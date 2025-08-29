
using HRManagement.Application.DTOs;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeRankService
    {
        public Task<EmployeeRankDto?> GetById(long id);
        public Task<List<EmployeeRankDto>> GetAll();
        public Task<List<EmployeeRankDto>> GetByEmployeeId(long employeeId);
        public Task<EmployeeRankDto> Create(CreateEmployeeRankDto createDto);
        public Task<EmployeeRankDto> Update(long id, UpdateEmployeeRankDto updateDto);
        public Task<List<EmployeeRankDto>> GetByRankId(long rankId);
        public Task Delete(long id);
        public Task<bool> ActiveExists(long id);
    }
}