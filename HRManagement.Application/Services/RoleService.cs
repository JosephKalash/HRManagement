using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Interfaces;
namespace HRManagement.Application.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync() { var roles = await _roleRepository.GetAllAsync(); return roles.Select(RoleMappers.MapToDto).ToList(); }
        public async Task<RoleDto?> GetRoleByIdAsync(Guid id) { var role = await _roleRepository.GetByIdAsync(id); return role == null ? null : RoleMappers.MapToDto(role); }
        public async Task<IEnumerable<RoleDto>> GetRolesByLevelAsync(RoleLevel level) { var roles = await _roleRepository.GetByLevelAsync(level); return roles.Select(RoleMappers.MapToDto).ToList(); }
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createDto)
        {
            var role = RoleMappers.MapToEntity(createDto);
            var createdRole = await _roleRepository.AddAsync(role);
            return RoleMappers.MapToDto(createdRole);
        }

        public async Task<RoleDto?> UpdateRoleAsync(Guid id, UpdateRoleDto updateDto)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);
            if (existingRole == null) return null;

            // Use the mapper to apply conditional updates
            var updatedRole = RoleMappers.MapToEntity(updateDto, existingRole);

            await _roleRepository.UpdateAsync(updatedRole);
            return RoleMappers.MapToDto(updatedRole);
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return false;

            await _roleRepository.DeleteAsync(role);
            return true;
        }
    }

    static class RoleMappers
    {
        public static RoleDto MapToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Code = role.Code,
                Description = role.Description,
                Level = role.Level,
                IsActive = role.IsActive,
                IsSystemRole = role.IsSystemRole
            };
        }

        public static Role MapToEntity(CreateRoleDto createRoleDto)
        {
            return new Role
            {
                Name = createRoleDto.Name,
                Code = createRoleDto.Code,
                Description = createRoleDto.Description,
                Level = createRoleDto.Level,
                IsSystemRole = createRoleDto.IsSystemRole
            };
        }

        public static Role MapToEntity(UpdateRoleDto updateRoleDto, Role existingRole)
        {
            // Apply updates conditionally based on provided fields
            if (!string.IsNullOrEmpty(updateRoleDto.Name))
            {
                existingRole.Name = updateRoleDto.Name;
            }

            if (!string.IsNullOrEmpty(updateRoleDto.Code))
            {
                existingRole.Code = updateRoleDto.Code;
            }

            if (!string.IsNullOrEmpty(updateRoleDto.Description))
            {
                existingRole.Description = updateRoleDto.Description;
            }

            if (updateRoleDto.Level.HasValue)
            {
                existingRole.Level = updateRoleDto.Level.Value;
            }

            if (updateRoleDto.IsActive.HasValue)
            {
                existingRole.IsActive = updateRoleDto.IsActive.Value;
            }

            return existingRole;
        }
    }

}