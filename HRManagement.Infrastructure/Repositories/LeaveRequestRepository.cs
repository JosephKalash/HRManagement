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

        public Task<IEnumerable<LeaveRequest>> GetByEmployeeId(Guid employeeId)
        {
            // return await _dbSet.Where(lr => lr.EmployeeId == employeeId).ToListAsync();
            throw new NotImplementedException("This method is not implemented yet.");
        }

        public async Task<IEnumerable<LeaveRequest>> GetByStatus(LeaveStatus status)
        {
            return await _dbSet.Where(lr => lr.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequests()
        {
            return await _dbSet.Where(lr => lr.Status == LeaveStatus.Pending).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(lr =>
                (lr.StartDate >= startDate && lr.StartDate <= endDate) ||
                (lr.EndDate >= startDate && lr.EndDate <= endDate) ||
                (lr.StartDate <= startDate && lr.EndDate >= endDate)
            ).ToListAsync();
        }
    }
}