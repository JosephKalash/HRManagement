using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByGuid(Guid guid);
        Task<Guid?> GetGuidById(long id);
        Task<List<Guid>> GetGuidsByIds(List<long> ids);
        Task<T?> GetById(long id);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<bool> ActiveExists(long id);
        IQueryable<T> AsQueryable();
    }
}