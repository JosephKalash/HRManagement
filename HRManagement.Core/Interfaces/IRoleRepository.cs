using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
// using HRManagement.Core.Enums;

namespace HRManagement.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetByLevelAsync(RoleLevel level);
    }
} 