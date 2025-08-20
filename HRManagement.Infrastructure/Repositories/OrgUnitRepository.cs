using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class OrgUnitRepository(HRDbContext context) : Repository<OrgUnit>(context), IOrgUnitRepository
    {
        public Task<List<OrgUnit>> GetByParentIdAsync(Guid? parentId)
        {
            return _dbSet.Where(o => o.ParentId == parentId).ToListAsync();
        }

        public Task<List<OrgUnit>> GetByTypeAsync(OrgUnitType type)
        {
            return _dbSet.Where(o => o.Type == type).ToListAsync();
        }

        public Task<List<OrgUnit>> SearchByNameAsync(string searchTerm)
        {
            return _dbSet.Where(o => o.Name.Contains(searchTerm)).ToListAsync();
        }

        public Task<List<OrgUnit>> GetAllWithChildrenAsync()
        {
            return _dbSet.Include(o => o.Children).ToListAsync();
        }

        public Task<List<OrgUnit>> GetChildUnitsAsync(Guid unitId)
        {
            return _dbSet
                    .Where(ou => ou.ParentId == unitId)
                    .ToListAsync();
        }
    }
}