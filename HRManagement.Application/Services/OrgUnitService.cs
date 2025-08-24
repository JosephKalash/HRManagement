using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class OrgUnitService(IOrgUnitRepository orgUnitRepository, IMapper mapper) : IOrgUnitService
    {
        private readonly IOrgUnitRepository _orgUnitRepository = orgUnitRepository;
        private readonly IMapper _mapper = mapper;

        public bool ValidateHierarchy(OrgUnit unit)
        {
            if (unit.Parent == null && unit.Type != OrgUnitType.LeaderOffice)
                return false; // Only leader office can be root

            if (unit.Parent != null)
            {
                var validTransitions = new Dictionary<OrgUnitType, List<OrgUnitType>>
                {
                    [OrgUnitType.LeaderOffice] = [OrgUnitType.ViceLeaderOffice, OrgUnitType.GeneralManagement],
                    [OrgUnitType.ViceLeaderOffice] = [OrgUnitType.GeneralManagement, OrgUnitType.Department],
                    [OrgUnitType.GeneralManagement] = [OrgUnitType.Department],
                    [OrgUnitType.Department] = [OrgUnitType.Section, OrgUnitType.Branch],
                    [OrgUnitType.Section] = [],
                    [OrgUnitType.Branch] = []
                };

                return validTransitions[unit.Parent.Type].Contains(unit.Type);
            }

            return true;
        }
        private static string GetHierarchyPath(OrgUnit unit)
        {
            var path = "";
            var current = unit;

            while (current != null)
            {
                path = string.Join(current.Name, path);
                current = current.Parent;
            }

            return path;
        }

        public async Task<List<OrgUnit>> GetAllChildUnitsAsync(Guid unitId)
        {
            var children = new List<OrgUnit>();
            var directChildren = await _orgUnitRepository.GetChildUnits(unitId);

            children.AddRange(directChildren);

            foreach (var child in directChildren)
            {
                var grandChildren = await GetAllChildUnitsAsync(child.Id);
                children.AddRange(grandChildren);
            }

            return children;
        }
        public int CalculateHierarchyLevel(OrgUnitType unitType)
        {
            return unitType switch
            {
                OrgUnitType.LeaderOffice => 0,
                OrgUnitType.ViceLeaderOffice => 1,
                OrgUnitType.GeneralManagement => 2,
                OrgUnitType.Department => 3,
                OrgUnitType.Section => 4,
                OrgUnitType.Branch => 4, // Same level as Section
                _ => 5
            };
        }
        static readonly Dictionary<OrgUnitType, List<OrgUnitType>> sameLevel = new()
        {
            [OrgUnitType.LeaderOffice] = [OrgUnitType.LeaderOffice],
            [OrgUnitType.ViceLeaderOffice] = [OrgUnitType.ViceLeaderOffice],
            [OrgUnitType.GeneralManagement] = [OrgUnitType.GeneralManagement],
            [OrgUnitType.Department] = [OrgUnitType.Department],
            [OrgUnitType.Section] = [OrgUnitType.Section, OrgUnitType.Branch],
            [OrgUnitType.Branch] = [OrgUnitType.Section, OrgUnitType.Branch]
        };
        // Check if units are at same hierarchical level
        public bool AreSameHierarchyLevel(OrgUnitType type1, OrgUnitType type2) => sameLevel[type1].Contains(type2);
        public async Task<OrgUnitDto?> GetById(Guid id)
        {
            var orgUnit = await _orgUnitRepository.GetById(id);
            return orgUnit != null ? _mapper.Map<OrgUnitDto>(orgUnit) : null;
        }

        public async Task<IEnumerable<OrgUnitDto>> GetAll()
        {
            var orgUnits = await _orgUnitRepository.GetAll();
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<IEnumerable<OrgUnitDto>> GetByParentId(Guid? parentId)
        {
            var orgUnits = await _orgUnitRepository.GetByParentId(parentId);
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<IEnumerable<OrgUnitDto>> GetByType(OrgUnitType type)
        {
            var orgUnits = await _orgUnitRepository.GetByType(type);
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<IEnumerable<OrgUnitDto>> SearchByName(string searchTerm)
        {
            var orgUnits = await _orgUnitRepository.SearchByName(searchTerm);
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<PagedResult<OrgUnitDto>> GetPaged(int pageNumber, int pageSize)
        {
            var query = _orgUnitRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<OrgUnitDto>>(paged.Items);
            return new PagedResult<OrgUnitDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<OrgUnitDto> Create(CreateOrgUnitDto dto)
        {
            var orgUnit = _mapper.Map<OrgUnit>(dto);
            if (!ValidateHierarchy(orgUnit))
                throw new ArgumentException("Invalid hierarchy for the organization unit");
            orgUnit.HierarchyPath = GetHierarchyPath(orgUnit);
            var created = await _orgUnitRepository.AddAsync(orgUnit);
            return _mapper.Map<OrgUnitDto>(created);
        }

        public async Task<OrgUnitDto> Update(Guid id, UpdateOrgUnitDto dto)
        {
            var orgUnit = await _orgUnitRepository.GetById(id);
            if (orgUnit == null)
                throw new ArgumentException("OrgUnit not found");

            _mapper.Map(dto, orgUnit);
            var updated = await _orgUnitRepository.Update(orgUnit);
            return _mapper.Map<OrgUnitDto>(updated);
        }

        public async Task Delete(Guid id)
        {
            var orgUnit = await _orgUnitRepository.GetById(id);
            if (orgUnit == null)
                throw new ArgumentException("OrgUnit not found");
            await _orgUnitRepository.Delete(orgUnit);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _orgUnitRepository.ActiveExists(id);
        }

        public async Task<OrgUnitHierarchyDto> GetHierarchy()
        {
            var allOrgUnits = await _orgUnitRepository.GetAllWithChildren();
            var leaderUnit = allOrgUnits.FirstOrDefault(o => o.ParentId == null);
            return _mapper.Map<OrgUnitHierarchyDto>(leaderUnit!);
        }
    }
}