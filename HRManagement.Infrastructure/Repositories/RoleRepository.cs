using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

// using HRManagement.Core.Enums;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HRManagement.Core.Extensions;
using HRManagement.Core.Models;

namespace HRManagement.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Role>> GetByLevelAsync(RoleLevel level)
        {
            return await _context.Roles
                .Where(r => r.Level == level)
                .ToListAsync();
        }

        public async Task<PagedResult<Role>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsNoTracking();
            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<Role>> GetPagedByLevelAsync(RoleLevel level, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(r => r.Level == level).AsNoTracking();
            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }
    }
}