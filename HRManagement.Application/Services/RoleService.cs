using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
// using HRManagement.Core.Enums;
using HRManagement.Core.Interfaces;
using AutoMapper;
using HRManagement.Core.Enums;

namespace HRManagement.Application.Services
{
    public class RoleService(IRoleRepository roleRepository, IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return role != null ? _mapper.Map<RoleDto>(role) : null;
        }

        public async Task<IEnumerable<RoleDto>> GetRolesByLevelAsync(RoleLevel level)
        {
            var roles = await _roleRepository.GetByLevelAsync(level);
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createDto)
        {
            var role = _mapper.Map<Role>(createDto);
            var createdRole = await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleDto>(createdRole);
        }

        public async Task<RoleDto?> UpdateRoleAsync(Guid id, UpdateRoleDto updateDto)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);
            if (existingRole == null) return null;

            _mapper.Map(updateDto, existingRole);
            var updatedRole = await _roleRepository.UpdateAsync(existingRole);
            return _mapper.Map<RoleDto>(updatedRole);
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return false;

            await _roleRepository.DeleteAsync(role);
            return true;
        }
    }
}