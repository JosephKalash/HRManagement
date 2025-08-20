using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Interfaces
{
    public interface IOrgUnitRepository : IRepository<OrgUnit>
    {
        Task<List<OrgUnit>> GetByParentIdAsync(Guid? parentId);
        Task<List<OrgUnit>> GetByTypeAsync(OrgUnitType type);
        Task<List<OrgUnit>> SearchByNameAsync(string searchTerm);
        Task<List<OrgUnit>> GetAllWithChildrenAsync();
        Task<List<OrgUnit>> GetChildUnitsAsync(Guid unitId);
    }
}