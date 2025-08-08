using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeAssignmentRepository : IRepository<EmployeeAssignment>
    {
        Task<IEnumerable<EmployeeAssignment>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeAssignment?> GetActiveByEmployeeIdAsync(Guid employeeId);
    }
} 