using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IOrgUnitService
    {
        public bool AreSameHierarchyLevel(OrgUnitType type1, OrgUnitType type2);
        public bool ValidateHierarchy(OrgUnit unit);
        public int CalculateHierarchyLevel(OrgUnitType unitType);
        public Task<OrgUnitDto?> GetById(long id);
        public Task<OrgUnitDto?> GetByGuid(Guid guid);
        public Task<IEnumerable<OrgUnitDto>> GetAll();
        public Task<IEnumerable<OrgUnitDto>> GetByParentId(long? parentId);
        public Task<IEnumerable<OrgUnitDto>> GetByType(OrgUnitType type);
        public Task<IEnumerable<OrgUnitDto>> SearchByName(string searchTerm);
        public Task<OrgUnitDto> Create(CreateOrgUnitDto dto);
        public Task<OrgUnitDto> Update(long id, UpdateOrgUnitDto dto);
        public Task Delete(long id);
        public Task<bool> Exists(long id);
        public Task<PagedResult<OrgUnitDto>> GetPaged(int pageNumber, int pageSize);
        Task<OrgUnitHierarchyDto> GetHierarchy();
    }
}