using HRManagement.Application.DTOs;
using HRManagement.Core.Models;

namespace HRManagement.Application.Interfaces
{
    public interface IRankService
    {
        Task<RankDto?> GetById(long id);
        Task<RankDto?> GetByGuid(Guid guid);
        Task<RankDto?> GetByOrder(int order);
        Task<PagedResult<RankDto>> GetPaged(int pageNumber, int pageSize);
        Task<RankDto> Create(CreateRankDto dto);
        Task<RankDto> Update(long id, UpdateRankDto dto);
        Task Delete(long id);
        Task<bool> Exists(long id);
    }
}


