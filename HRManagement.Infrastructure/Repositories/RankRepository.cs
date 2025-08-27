using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class RankRepository(HRDbContext context) : Repository<Rank>(context), IRankRepository
    {
        public Task<Rank?> GetByOrder(int order)
        {
            return _dbSet.FirstOrDefaultAsync(r => r.Order == order);
        }
    }
}


