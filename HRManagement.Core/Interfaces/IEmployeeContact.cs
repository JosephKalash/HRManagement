using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Core.Repositories
{
    public interface IEmployeeContactRepository : IRepository<EmployeeContact>
    {
        Task<EmployeeContact?> GetEmployeeContactByEmployeeId(Guid employeeId);
        Task<PagedResult<EmployeeContact>> GetPagedAsync(int pageNumber, int pageSize);
    }
}