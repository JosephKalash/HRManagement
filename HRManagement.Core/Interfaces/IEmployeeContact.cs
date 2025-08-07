using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;

namespace HRManagement.Core.Repositories
{
    public interface IEmployeeContactRepository : IRepository<EmployeeContact>
    {
        Task<EmployeeContact?> GetEmployeeContactByEmployeeId(Guid employeeId);
    }
}