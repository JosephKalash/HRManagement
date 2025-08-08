using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HRManagement.Core.Extensions;
using HRManagement.Core.Models;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeServiceInfoRepository : Repository<EmployeeServiceInfo>, IEmployeeServiceInfoRepository
    {
        public EmployeeServiceInfoRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmployeeServiceInfo>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeServiceInfos
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .Where(esi => esi.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<EmployeeServiceInfo?> GetActiveByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeServiceInfos
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .FirstOrDefaultAsync(esi => esi.EmployeeId == employeeId && esi.IsActive);
        }

        public async Task<PagedResult<EmployeeServiceInfo>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .AsNoTracking();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<EmployeeServiceInfo>> GetPagedByEmployeeIdAsync(Guid employeeId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(esi => esi.Employee)
                .Include(esi => esi.BelongingUnit)
                .Include(esi => esi.JobRole)
                .Where(esi => esi.EmployeeId == employeeId)
                .AsNoTracking();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }
    }
} 