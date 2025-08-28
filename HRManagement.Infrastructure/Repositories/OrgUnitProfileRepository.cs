using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class OrgUnitProfileRepository(HRDbContext context) : Repository<OrgUnitProfile>(context), IOrgUnitProfileRepository
    {
        public Task<OrgUnitProfile?> GetByOrgUnitId(long orgUnitId)
        {
            return _dbSet.FirstOrDefaultAsync(p => p.OrgUnitId == orgUnitId);
        }
    }
}



