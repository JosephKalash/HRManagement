using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class OrgUnitRepository : Repository<OrgUnit>, IOrgUnitRepository
    {
        public OrgUnitRepository(HRDbContext context) : base(context) { }

        public async Task<IEnumerable<OrgUnit>> GetByParentIdAsync(Guid? parentId)
        {
            return await _dbSet.Where(o => o.ParentId == parentId).ToListAsync();
        }

        public async Task<IEnumerable<OrgUnit>> GetByTypeAsync(OrgUnitType type)
        {
            return await _dbSet.Where(o => o.Type == type).ToListAsync();
        }

        public async Task<IEnumerable<OrgUnit>> SearchByNameAsync(string searchTerm)
        {
            return await _dbSet.Where(o => o.Name.Contains(searchTerm)).ToListAsync();
        }
    }
} 