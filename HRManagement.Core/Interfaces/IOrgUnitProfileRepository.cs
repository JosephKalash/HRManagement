using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IOrgUnitProfileRepository : IRepository<OrgUnitProfile>
    {
        Task<OrgUnitProfile?> GetByOrgUnitId(long orgUnitId);
    }
}


