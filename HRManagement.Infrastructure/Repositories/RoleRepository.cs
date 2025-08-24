using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

// using HRManagement.Core.Enums;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Role>> GetByLevel(RoleLevel level)
        {
            return await _context.Roles
                .Where(r => r.Level == level)
                .ToListAsync();
        }
    }
}