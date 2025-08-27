using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IOrgUnitProfileService
    {
        Task<OrgUnitProfileDto?> GetById(long id);
        Task<OrgUnitProfileDto?> GetByOrgUnitId(long orgUnitId);
        Task<PagedResult<OrgUnitProfileDto>> GetPaged(int pageNumber, int pageSize);
        Task<OrgUnitProfileDto> Create(CreateOrgUnitProfileDto dto);
        Task<OrgUnitProfileDto> Update(long id, UpdateOrgUnitProfileDto dto);
        Task Delete(long id);
    }
}


