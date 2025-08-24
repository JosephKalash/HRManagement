using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface ILeaveRequestRepository : IRepository<LeaveRequest>
    {
        Task<IEnumerable<LeaveRequest>> GetByEmployeeId(Guid employeeId);
        Task<IEnumerable<LeaveRequest>> GetByStatus(LeaveStatus status);
        Task<IEnumerable<LeaveRequest>> GetPendingRequests();
        Task<IEnumerable<LeaveRequest>> GetByDateRange(DateTime startDate, DateTime endDate);
    }
}