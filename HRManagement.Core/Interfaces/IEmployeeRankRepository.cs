
using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeRankRepository : IRepository<EmployeeRank>
    {
        Task<List<EmployeeRank>> GetByEmployeeId(long employeeId);
        Task<List<EmployeeRank>> GetByRankId(long RankId);
    }
}
