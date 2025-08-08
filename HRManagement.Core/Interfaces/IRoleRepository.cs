using HRManagement.Core.Entities;
using HRManagement.Core.Models;

namespace HRManagement.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetByLevelAsync(RoleLevel level);
        Task<PagedResult<Role>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagedResult<Role>> GetPagedByLevelAsync(RoleLevel level, int pageNumber, int pageSize);
    }
} 