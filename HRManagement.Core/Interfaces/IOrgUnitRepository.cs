using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IOrgUnitRepository : IRepository<OrgUnit>
    {
        Task<IEnumerable<OrgUnit>> GetByParentIdAsync(Guid? parentId);
        Task<IEnumerable<OrgUnit>> GetByTypeAsync(OrgUnitType type);
        Task<IEnumerable<OrgUnit>> SearchByNameAsync(string searchTerm);
    }
} 