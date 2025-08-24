using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class Repository<T>(HRDbContext context) : IRepository<T> where T : BaseEntity
    {
        protected readonly HRDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public virtual async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ActiveExists(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
}