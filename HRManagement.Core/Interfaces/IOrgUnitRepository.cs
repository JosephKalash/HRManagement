using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Interfaces
{
    public interface IOrgUnitRepository : IRepository<OrgUnit>
    {
        Task<List<OrgUnit>> GetByParentId(long? parentId);
        Task<List<OrgUnit>> GetByType(OrgUnitType type);
        Task<List<OrgUnit>> SearchByName(string searchTerm);
        Task<List<OrgUnit>> GetAllWithChildren();
        Task<List<OrgUnit>> GetChildUnits(long unitId);
    }
}