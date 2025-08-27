using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeServiceInfoRepository : IRepository<EmployeeServiceInfo>
    {
        Task<IEnumerable<EmployeeServiceInfo>> GetByEmployeeId(long employeeId);
        Task<EmployeeServiceInfo?> GetActiveByEmployeeId(long employeeId);
        Task<IEnumerable<EmployeeServiceInfo>> GetByRoleId(long roleId);
    }
}