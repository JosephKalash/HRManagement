using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeServiceInfoRepository : IRepository<EmployeeServiceInfo>
    {
        Task<IEnumerable<EmployeeServiceInfo>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeServiceInfo?> GetActiveByEmployeeIdAsync(Guid employeeId);
    }
} 