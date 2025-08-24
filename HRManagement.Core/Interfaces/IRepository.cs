using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<bool> ActiveExists(Guid id);
        IQueryable<T> AsQueryable();
    }
}