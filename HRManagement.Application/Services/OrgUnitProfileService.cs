using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class OrgUnitProfileService(IOrgUnitProfileRepository profileRepository, IOrgUnitRepository orgUnitRepository, IMapper mapper) : IOrgUnitProfileService
    {
        private readonly IOrgUnitProfileRepository _profileRepository = profileRepository;
        private readonly IOrgUnitRepository _orgUnitRepository = orgUnitRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OrgUnitProfileDto?> GetById(long id)
        {
            var profile = await _profileRepository.GetById(id);
            return profile != null ? _mapper.Map<OrgUnitProfileDto>(profile) : null;
        }

        public async Task<OrgUnitProfileDto?> GetByOrgUnitId(long orgUnitId)
        {
            var profile = await _profileRepository.GetByOrgUnitId(orgUnitId);
            return profile != null ? _mapper.Map<OrgUnitProfileDto>(profile) : null;
        }

        public async Task<PagedResult<OrgUnitProfileDto>> GetPaged(int pageNumber, int pageSize)
        {
            var query = _profileRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtos = _mapper.Map<List<OrgUnitProfileDto>>(paged.Items);
            return new PagedResult<OrgUnitProfileDto>
            {
                Items = dtos,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<OrgUnitProfileDto> Create(CreateOrgUnitProfileDto dto)
        {
            var orgUnit = await _orgUnitRepository.GetById(dto.OrgUnitId);
            if (orgUnit == null)
                throw new ArgumentException($"OrgUnit with ID {dto.OrgUnitId} not found");

            var existing = await _profileRepository.GetByOrgUnitId(dto.OrgUnitId);
            if (existing != null)
                throw new ArgumentException($"Profile already exists for OrgUnit {dto.OrgUnitId}");

            var entity = _mapper.Map<OrgUnitProfile>(dto);
            var created = await _profileRepository.AddAsync(entity);
            return _mapper.Map<OrgUnitProfileDto>(created);
        }

        public async Task<OrgUnitProfileDto> Update(long id, UpdateOrgUnitProfileDto dto)
        {
            var profile = await _profileRepository.GetById(id);
            if (profile == null)
                throw new ArgumentException($"OrgUnitProfile with ID {id} not found");

            if (dto.OrgUnitId.HasValue && dto.OrgUnitId.Value != profile.OrgUnitId)
            {
                var orgUnit = await _orgUnitRepository.GetById(dto.OrgUnitId.Value);
                if (orgUnit == null)
                    throw new ArgumentException($"OrgUnit with ID {dto.OrgUnitId} not found");
                var other = await _profileRepository.GetByOrgUnitId(dto.OrgUnitId.Value);
                if (other != null && other.Id != profile.Id)
                    throw new ArgumentException($"Another profile already exists for OrgUnit {dto.OrgUnitId}");
            }

            _mapper.Map(dto, profile);
            profile.UpdatedAt = DateTime.UtcNow;
            var updated = await _profileRepository.Update(profile);
            return _mapper.Map<OrgUnitProfileDto>(updated);
        }

        public async Task Delete(long id)
        {
            var profile = await _profileRepository.GetById(id);
            if (profile == null)
                throw new ArgumentException($"OrgUnitProfile with ID {id} not found");

            await _profileRepository.Delete(profile);
        }
    }
}


