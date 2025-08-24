using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetByLevel(RoleLevel level);
    }
}