using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeServiceInfoRepository : Repository<EmployeeServiceInfo>, IEmployeeServiceInfoRepository
    {
        public EmployeeServiceInfoRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmployeeServiceInfo>> GetByEmployeeId(Guid employeeId)
        {
            return await _context.EmployeeServiceInfos
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .Where(esi => esi.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<EmployeeServiceInfo?> GetActiveByEmployeeId(Guid employeeId)
        {
            return await _context.EmployeeServiceInfos
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .FirstOrDefaultAsync(esi => esi.EmployeeId == employeeId && esi.IsActive);
        }

        public async Task<IEnumerable<EmployeeServiceInfo>> GetByRoleId(Guid roleId)
        {
            return await _context.EmployeeServiceInfos
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .Where(esi => esi.JobRoleId == roleId && esi.IsActive)
                .ToListAsync();
        }
    }
}