using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeAssignmentRepository(HRDbContext context) : Repository<EmployeeAssignment>(context), IEmployeeAssignmentRepository
    {
        public async Task<List<EmployeeAssignment>> GetByEmployeeId(Guid employeeId)
        {
            return await _context.EmployeeAssignments
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .Where(ea => ea.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<EmployeeAssignment?> GetActiveByEmployeeId(Guid employeeId)
        {
            return await _context.EmployeeAssignments
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .FirstOrDefaultAsync(ea => ea.EmployeeId == employeeId && ea.IsActive);
        }

        public async Task<List<EmployeeAssignment>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.EmployeeAssignments
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .Where(ea => ea.JobRoleId == roleId && ea.IsActive)
                .ToListAsync();
        }

        public async Task<List<EmployeeAssignment>> GetByUnitId(Guid unitId)
        {
            return await _dbSet
                .Include(ea => ea.Employee)
                .Include(ea => ea.AssignedUnit)
                .Include(ea => ea.JobRole)
                .Where(ea => ea.AssignedUnitId == unitId && ea.IsActive)
                .ToListAsync();
        }
    }
}