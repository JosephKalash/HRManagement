using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class OrgUnitService(IOrgUnitRepository orgUnitRepository) : IOrgUnitService
    {
        private readonly IOrgUnitRepository _orgUnitRepository = orgUnitRepository;

        public async Task<OrgUnitDto?> GetByIdAsync(Guid id)
        {
            var orgUnit = await _orgUnitRepository.GetByIdAsync(id);
            return orgUnit != null ? MapToDto(orgUnit) : null;
        }

        public async Task<IEnumerable<OrgUnitDto>> GetAllAsync()
        {
            var orgUnits = await _orgUnitRepository.GetAllAsync();
            return orgUnits.Select(MapToDto);
        }

        public async Task<IEnumerable<OrgUnitDto>> GetByParentIdAsync(Guid? parentId)
        {
            var orgUnits = await _orgUnitRepository.GetByParentIdAsync(parentId);
            return orgUnits.Select(MapToDto);
        }

        public async Task<IEnumerable<OrgUnitDto>> GetByTypeAsync(OrgUnitType type)
        {
            var orgUnits = await _orgUnitRepository.GetByTypeAsync(type);
            return orgUnits.Select(MapToDto);
        }

        public async Task<IEnumerable<OrgUnitDto>> SearchByNameAsync(string searchTerm)
        {
            var orgUnits = await _orgUnitRepository.SearchByNameAsync(searchTerm);
            return orgUnits.Select(MapToDto);
        }

        public async Task<PagedResult<OrgUnitDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _orgUnitRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = paged.Items.Select(MapToDto).ToList();
            return new PagedResult<OrgUnitDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<OrgUnitDto> CreateAsync(CreateOrgUnitDto dto)
        {
            var orgUnit = new OrgUnit
            {
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                ParentId = dto.ParentId,
                ManagerId = dto.ManagerId
            };
            var created = await _orgUnitRepository.AddAsync(orgUnit);
            return MapToDto(created);
        }

        public async Task<OrgUnitDto> UpdateAsync(Guid id, UpdateOrgUnitDto dto)
        {
            var orgUnit = await _orgUnitRepository.GetByIdAsync(id);
            if (orgUnit == null)
                throw new ArgumentException("OrgUnit not found");
            orgUnit.Name = dto.Name ?? orgUnit.Name;
            orgUnit.Type = dto.Type ?? orgUnit.Type;
            orgUnit.Description = dto.Description;
            orgUnit.ParentId = dto.ParentId;
            orgUnit.ManagerId = dto.ManagerId;
            var updated = await _orgUnitRepository.UpdateAsync(orgUnit);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            var orgUnit = await _orgUnitRepository.GetByIdAsync(id);
            if (orgUnit == null)
                throw new ArgumentException("OrgUnit not found");
            await _orgUnitRepository.DeleteAsync(orgUnit);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _orgUnitRepository.ExistsAsync(id);
        }

        public async Task<OrgUnitHierarchyDto> GetHierarchyAsync()
        {
            var allOrgUnits = await _orgUnitRepository.GetAllWithChildrenAsync();

            var rootUnits = allOrgUnits.Where(o => o.ParentId == null).FirstOrDefault();
            // var hierarchy = rootUnits.Select(MapToHierarchyDto);

            return MapToHierarchyDto(rootUnits!);
        }

        private static OrgUnitDto MapToDto(OrgUnit orgUnit)
        {
            return new OrgUnitDto
            {
                Id = orgUnit.Id,
                Name = orgUnit.Name,
                Type = orgUnit.Type,
                Description = orgUnit.Description,
                ParentId = orgUnit.ParentId,
                ManagerId = orgUnit.ManagerId,
                Children = orgUnit.Children?.Select(MapToDto).ToList() ?? new List<OrgUnitDto>()
            };
        }

        private OrgUnitHierarchyDto MapToHierarchyDto(OrgUnit orgUnit)
        {
            return new OrgUnitHierarchyDto
            {
                Id = orgUnit.Id,
                Name = orgUnit.Name,
                ManagerId = orgUnit.ManagerId,
                Children = orgUnit.Children?.Select(MapToHierarchyDto).ToList() ?? new List<OrgUnitHierarchyDto>()
            };
        }
    }
}