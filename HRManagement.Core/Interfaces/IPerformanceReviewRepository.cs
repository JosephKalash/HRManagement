using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IPerformanceReviewRepository : IRepository<PerformanceReview>
    {
        Task<IEnumerable<PerformanceReview>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<PerformanceReview>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<PerformanceReview>> GetByRatingAsync(int minRating, int maxRating);
    }
} 