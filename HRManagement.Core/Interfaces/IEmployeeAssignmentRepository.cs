using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeAssignmentRepository : IRepository<EmployeeAssignment>
    {
        Task<List<EmployeeAssignment>> GetByEmployeeId(long employeeId);
        Task<EmployeeAssignment?> GetActiveByEmployeeId(long employeeId);
        Task<List<EmployeeAssignment>> GetByRoleId(long roleId);
        Task<List<EmployeeAssignment>> GetByUnitId(long unitId);
    }
}