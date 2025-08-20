using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeAssignmentRepository : IRepository<EmployeeAssignment>
    {
        Task<List<EmployeeAssignment>> GetByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeAssignment?> GetActiveByEmployeeIdAsync(Guid employeeId);
        Task<List<EmployeeAssignment>> GetByRoleIdAsync(Guid roleId);
        Task<List<EmployeeAssignment>> GetByUnitIdAsync(Guid unitId);
    }
} 