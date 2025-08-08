using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeServiceInfoRepository : IRepository<EmployeeServiceInfo>
    {
        Task<IEnumerable<EmployeeServiceInfo>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeServiceInfo?> GetActiveByEmployeeIdAsync(Guid employeeId);
        Task<PagedResult<EmployeeServiceInfo>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagedResult<EmployeeServiceInfo>> GetPagedByEmployeeIdAsync(Guid employeeId, int pageNumber, int pageSize);
    }
} 