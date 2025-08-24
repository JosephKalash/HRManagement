using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Models;
// using HRManagement.Core.Enums;

namespace HRManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles();
        Task<RoleDto?> GetRoleById(Guid id);
        Task<IEnumerable<RoleDto>> GetRolesByLevel(RoleLevel level);
        Task<RoleDto> Create(CreateRoleDto createDto);
        Task<RoleDto?> UpdateRole(Guid id, UpdateRoleDto updateDto);
        Task<bool> DeleteRole(Guid id);
        Task<bool> ActiveExistsRole(Guid id);
        Task<PagedResult<RoleDto>> GetPagedRoles(int pageNumber, int pageSize);
        Task<PagedResult<RoleDto>> GetPagedRolesByLevel(RoleLevel level, int pageNumber, int pageSize);
    }
}