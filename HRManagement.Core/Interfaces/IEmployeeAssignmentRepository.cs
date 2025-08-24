using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeAssignmentRepository : IRepository<EmployeeAssignment>
    {
        Task<List<EmployeeAssignment>> GetByEmployeeId(Guid employeeId);
        Task<EmployeeAssignment?> GetActiveByEmployeeId(Guid employeeId);
        Task<List<EmployeeAssignment>> GetByRoleIdAsync(Guid roleId);
        Task<List<EmployeeAssignment>> GetByUnitId(Guid unitId);
    }
}