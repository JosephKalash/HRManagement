using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeServiceInfoRepository : IRepository<EmployeeServiceInfo>
    {
        Task<IEnumerable<EmployeeServiceInfo>> GetByEmployeeId(Guid employeeId);
        Task<EmployeeServiceInfo?> GetActiveByEmployeeId(Guid employeeId);
        Task<IEnumerable<EmployeeServiceInfo>> GetByRoleId(Guid roleId);
    }
}