using HRManagement.Core.Entities;
using HRManagement.Core.Repositories;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeContactRepository(HRDbContext context) : Repository<EmployeeContact>(context), IEmployeeContactRepository
    {
        public async Task<EmployeeContact?> GetEmployeeContactByEmployeeId(long employeeId)
        {
            return await _context.EmployeeContacts
                .Include(ep => ep.Employee)
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId);
        }
    }
}