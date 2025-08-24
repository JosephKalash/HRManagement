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
        public Task<OrgUnitDto?> GetById(Guid id);
        public Task<IEnumerable<OrgUnitDto>> GetAll();
        public Task<IEnumerable<OrgUnitDto>> GetByParentId(Guid? parentId);
        public Task<IEnumerable<OrgUnitDto>> GetByType(OrgUnitType type);
        public Task<IEnumerable<OrgUnitDto>> SearchByName(string searchTerm);
        public Task<OrgUnitDto> Create(CreateOrgUnitDto dto);
        public Task<OrgUnitDto> Update(Guid id, UpdateOrgUnitDto dto);
        public Task Delete(Guid id);
        public Task<bool> Exists(Guid id);
        public Task<PagedResult<OrgUnitDto>> GetPaged(int pageNumber, int pageSize);
        Task<OrgUnitHierarchyDto> GetHierarchy();
    }
}