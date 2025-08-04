using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IOrgUnitService
    {
        Task<OrgUnitDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrgUnitDto>> GetAllAsync();
        Task<IEnumerable<OrgUnitDto>> GetByParentIdAsync(Guid? parentId);
        Task<IEnumerable<OrgUnitDto>> GetByTypeAsync(int type);
        Task<IEnumerable<OrgUnitDto>> SearchByNameAsync(string searchTerm);
        Task<OrgUnitDto> CreateAsync(OrgUnitDto dto);
        Task<OrgUnitDto> UpdateAsync(Guid id, OrgUnitDto dto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<OrgUnitDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
} 