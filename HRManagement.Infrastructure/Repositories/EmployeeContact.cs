using HRManagement.Core.Entities;
using HRManagement.Core.Repositories;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HRManagement.Core.Extensions;
using HRManagement.Core.Models;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeContactRepository(HRDbContext context) : Repository<EmployeeContact>(context), IEmployeeContactRepository
    {
        public async Task<EmployeeContact?> GetEmployeeContactByEmployeeId(Guid employeeId)
        {
            return await _context.EmployeeContacts
                .Include(ep => ep.Employee)
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId);
        }

        public async Task<PagedResult<EmployeeContact>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(ec => ec.Employee)
                .AsNoTracking();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }
    }
}