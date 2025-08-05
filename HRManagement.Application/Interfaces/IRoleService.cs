using HRManagement.Application.DTOs;
using HRManagement.Core.Enums;

namespace HRManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(Guid id);
        Task<IEnumerable<RoleDto>> GetRolesByLevelAsync(RoleLevel level);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createDto);
        Task<RoleDto?> UpdateRoleAsync(Guid id, UpdateRoleDto updateDto);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}