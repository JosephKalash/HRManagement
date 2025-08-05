using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeProfileRepository : Repository<EmployeeProfile>, IEmployeeProfileRepository
    {
        public EmployeeProfileRepository(HRDbContext context) : base(context)
        {
        }

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
    }
}