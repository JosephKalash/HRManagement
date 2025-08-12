using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeRepository(HRDbContext context) : Repository<Employee>(context), IEmployeeRepository
    {
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

        public async Task<Employee?> GetEmployeeWithAllDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(e => e.Profile)
                .Include(e => e.Contact)
                .Include(e => e.Signature)
                .Include(e => e.ServiceInfos)
                    .ThenInclude(si => si.JobRole)
                .Include(e => e.Assignments)
                    .ThenInclude(a => a.AssignedUnit)
                .Include(e => e.Assignments)
                    .ThenInclude(a => a.JobRole)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        IQueryable<Employee> IRepository<Employee>.AsQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
} 