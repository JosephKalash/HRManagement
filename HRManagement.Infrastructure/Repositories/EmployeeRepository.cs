using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<Employee?> GetByMilitaryNumberAsync(int militaryNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.MilitaryNumber == militaryNumber);
        }

        public async Task<Employee?> GetByIdNumberAsync(string idNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.IdNumber == idNumber);
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _dbSet.Where(e => e.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            return await _dbSet.Where(e => 
                e.ArabicFirstName.Contains(searchTerm) || 
                e.ArabicLastName.Contains(searchTerm) || 
                e.EnglishFirstName.Contains(searchTerm) ||
                e.EnglishLastName.Contains(searchTerm) ||
                e.IdNumber.Contains(searchTerm)
            ).ToListAsync();
        }

        IQueryable<Employee> IRepository<Employee>.AsQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
} 