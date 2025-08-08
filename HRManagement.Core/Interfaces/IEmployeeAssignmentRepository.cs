using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeAssignmentRepository : IRepository<EmployeeAssignment>
    {
        Task<IEnumerable<EmployeeAssignment>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeAssignment?> GetActiveByEmployeeIdAsync(Guid employeeId);
        Task<PagedResult<EmployeeAssignment>> GetPagedAsync(int pageNumber, int pageSize);
    }
} 