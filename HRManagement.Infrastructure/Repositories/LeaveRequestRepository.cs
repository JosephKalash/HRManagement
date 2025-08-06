using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class LeaveRequestRepository : Repository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HRDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(Guid employeeId)
        {
            // return await _dbSet.Where(lr => lr.EmployeeId == employeeId).ToListAsync();
            throw new NotImplementedException("This method is not implemented yet.");
        }

        public async Task<IEnumerable<LeaveRequest>> GetByStatusAsync(LeaveStatus status)
        {
            return await _dbSet.Where(lr => lr.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
        {
            return await _dbSet.Where(lr => lr.Status == LeaveStatus.Pending).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(lr => 
                (lr.StartDate >= startDate && lr.StartDate <= endDate) ||
                (lr.EndDate >= startDate && lr.EndDate <= endDate) ||
                (lr.StartDate <= startDate && lr.EndDate >= endDate)
            ).ToListAsync();
        }
    }
} 