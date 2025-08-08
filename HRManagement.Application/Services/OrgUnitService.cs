using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;
using AutoMapper;
using HRManagement.Core.Enums;

namespace HRManagement.Application.Services
{
    public class OrgUnitService(IOrgUnitRepository orgUnitRepository, IMapper mapper) : IOrgUnitService
    {
        private readonly IOrgUnitRepository _orgUnitRepository = orgUnitRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OrgUnitDto?> GetByIdAsync(Guid id)
        {
            var orgUnit = await _orgUnitRepository.GetByIdAsync(id);
            return orgUnit != null ? _mapper.Map<OrgUnitDto>(orgUnit) : null;
        }

        public async Task<IEnumerable<OrgUnitDto>> GetAllAsync()
        {
            var orgUnits = await _orgUnitRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<IEnumerable<OrgUnitDto>> GetByParentIdAsync(Guid? parentId)
        {
            var orgUnits = await _orgUnitRepository.GetByParentIdAsync(parentId);
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<IEnumerable<OrgUnitDto>> GetByTypeAsync(OrgUnitType type)
        {
            var orgUnits = await _orgUnitRepository.GetByTypeAsync(type);
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<IEnumerable<OrgUnitDto>> SearchByNameAsync(string searchTerm)
        {
            var orgUnits = await _orgUnitRepository.SearchByNameAsync(searchTerm);
            return _mapper.Map<IEnumerable<OrgUnitDto>>(orgUnits);
        }

        public async Task<PagedResult<OrgUnitDto>> GetPagedAsync(int pageNumber, int pageSize)
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

        public async Task<OrgUnitDto> CreateAsync(CreateOrgUnitDto dto)
        {
            var orgUnit = _mapper.Map<OrgUnit>(dto);
            var created = await _orgUnitRepository.AddAsync(orgUnit);
            return _mapper.Map<OrgUnitDto>(created);
        }

        public async Task<OrgUnitDto> UpdateAsync(Guid id, UpdateOrgUnitDto dto)
        {
            var orgUnit = await _orgUnitRepository.GetByIdAsync(id);
            if (orgUnit == null)
                throw new ArgumentException("OrgUnit not found");
            
            _mapper.Map(dto, orgUnit);
            var updated = await _orgUnitRepository.UpdateAsync(orgUnit);
            return _mapper.Map<OrgUnitDto>(updated);
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
            var leaderUnit = allOrgUnits.Where(o => o.ParentId == null).FirstOrDefault();
            return _mapper.Map<OrgUnitHierarchyDto>(leaderUnit!);
        }
    }
}