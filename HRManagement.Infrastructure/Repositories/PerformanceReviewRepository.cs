using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class PerformanceReviewRepository : Repository<PerformanceReview>, IPerformanceReviewRepository
    {
        public PerformanceReviewRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PerformanceReview>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.Where(pr => pr.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<IEnumerable<PerformanceReview>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(pr => pr.ReviewDate >= startDate && pr.ReviewDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<PerformanceReview>> GetByRatingAsync(int minRating, int maxRating)
        {
            return await _dbSet.Where(pr => pr.OverallRating >= minRating && pr.OverallRating <= maxRating).ToListAsync();
        }
    }
} 