using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HRManagement.Core.Extensions;
using HRManagement.Core.Models;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeProfileRepository(HRDbContext context) : Repository<EmployeeProfile>(context), IEmployeeProfileRepository
    {
        public async Task<EmployeeProfile?> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeProfiles
                .Include(ep => ep.Employee)
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId);
        }

        public async Task UpdateEmployeeImageAsync(Guid employeeId, string imagePath)
        {
            var employeeProfile = await _dbSet.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (employeeProfile == null)
            {
                throw new KeyNotFoundException("Employee profile not found.");
            }

            employeeProfile.ImagePath = imagePath;
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<EmployeeProfile>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(ep => ep.Employee)
                .AsNoTracking();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }
    }
}