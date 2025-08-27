using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IRankRepository : IRepository<Rank>
    {
        Task<Rank?> GetByOrder(int order);
    }
}


