using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetByLevelAsync(int level);
    }
} 