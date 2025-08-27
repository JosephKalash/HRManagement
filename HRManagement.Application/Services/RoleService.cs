using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class RoleService(IRoleRepository roleRepository, IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAll();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<PagedResult<RoleDto>> GetPagedRoles(int pageNumber, int pageSize)
        {
            var query = _roleRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<RoleDto>>(paged.Items);
            return new PagedResult<RoleDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<PagedResult<RoleDto>> GetPagedRolesByLevel(RoleLevel level, int pageNumber, int pageSize)
        {
            var query = _roleRepository.AsQueryable();
            // .Where(r => r.Level == level);
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<RoleDto>>(paged.Items);
            return new PagedResult<RoleDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<RoleDto?> GetRoleById(long id)
        {
            var role = await _roleRepository.GetById(id);
            return role != null ? _mapper.Map<RoleDto>(role) : null;
        }

        public async Task<IEnumerable<RoleDto>> GetRolesByLevel(RoleLevel level)
        {
            var roles = await _roleRepository.GetByLevel(level);
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> Create(CreateRoleDto createDto)
        {
            var role = _mapper.Map<Role>(createDto);
            var createdRole = await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleDto>(createdRole);
        }

        public async Task<RoleDto?> UpdateRole(long id, UpdateRoleDto updateDto)
        {
            var existingRole = await _roleRepository.GetById(id);
            if (existingRole == null) return null;

            _mapper.Map(updateDto, existingRole);
            var updatedRole = await _roleRepository.Update(existingRole);
            return _mapper.Map<RoleDto>(updatedRole);
        }

        public async Task<bool> DeleteRole(long id)
        {
            var role = await _roleRepository.GetById(id);
            if (role == null) return false;

            await _roleRepository.Delete(role);
            return true;
        }

        public async Task<bool> ActiveExistsRole(long id)
        {
            var isExist = await _roleRepository.ActiveExists(id);

            return isExist;
        }

        public async Task<RoleDto?> GetRoleByGuid(Guid id)
        {
            var role = await _roleRepository.GetByGuid(id);
            return role != null ? _mapper.Map<RoleDto>(role) : null;
        }
    }
}