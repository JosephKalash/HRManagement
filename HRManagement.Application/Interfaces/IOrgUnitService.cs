using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IOrgUnitService
    {
        public bool AreSameHierarchyLevel(OrgUnitType type1, OrgUnitType type2);
        public bool ValidateHierarchyAsync(OrgUnit unit);
        public int CalculateHierarchyLevel(OrgUnitType unitType);
        public Task<OrgUnitDto?> GetByIdAsync(Guid id);
        public Task<IEnumerable<OrgUnitDto>> GetAllAsync();
        public Task<IEnumerable<OrgUnitDto>> GetByParentIdAsync(Guid? parentId);
        public Task<IEnumerable<OrgUnitDto>> GetByTypeAsync(OrgUnitType type);
        public Task<IEnumerable<OrgUnitDto>> SearchByNameAsync(string searchTerm);
        public Task<OrgUnitDto> CreateAsync(CreateOrgUnitDto dto);
        public Task<OrgUnitDto> UpdateAsync(Guid id, UpdateOrgUnitDto dto);
        public Task DeleteAsync(Guid id);
        public Task<bool> ExistsAsync(Guid id);
        public Task<PagedResult<OrgUnitDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<OrgUnitHierarchyDto> GetHierarchyAsync();
    }
}