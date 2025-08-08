using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeProfileRepository : IRepository<EmployeeProfile>
    {
        Task<EmployeeProfile?> GetByEmployeeIdAsync(Guid employeeId);
        Task UpdateEmployeeImageAsync(Guid employeeId, string imagePath);
        Task<PagedResult<EmployeeProfile>> GetPagedAsync(int pageNumber, int pageSize);
    }
}