using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeRankRepository(HRDbContext context) : Repository<EmployeeRank>(context), IEmployeeRankRepository
    {
        public async Task<List<EmployeeRank>> GetByEmployeeId(long employeeId)
        {
            return await _context.EmployeeRanks
                .Where(e => e.EmployeeId == employeeId).Include(e => e.Rank)
                .ToListAsync();
        }

        public async Task<List<EmployeeRank>> GetByRankId(long rankId)
        {
            return await _context.EmployeeRanks
                .Where(e => e.RankId == rankId)
                .Include(e => e.Employee)
                .ToListAsync();
        }
    }
}
