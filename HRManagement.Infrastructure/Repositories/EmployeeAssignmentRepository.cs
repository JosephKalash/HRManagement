using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeAssignmentRepository : Repository<EmployeeAssignment>, IEmployeeAssignmentRepository
    {
        public EmployeeAssignmentRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmployeeAssignment>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeAssignments
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .Where(ea => ea.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<EmployeeAssignment?> GetActiveByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeAssignments
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .FirstOrDefaultAsync(ea => ea.EmployeeId == employeeId && ea.IsActive);
        }

        public async Task<IEnumerable<EmployeeAssignment>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.EmployeeAssignments
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .Where(ea => ea.JobRoleId == roleId && ea.IsActive)
                .ToListAsync();
        }
    }
} 