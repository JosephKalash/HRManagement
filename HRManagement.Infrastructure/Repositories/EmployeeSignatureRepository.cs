using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeSignatureRepository(HRDbContext context) : Repository<EmployeeSignature>(context), IEmployeeSignatureRepository
    {
        public async Task<EmployeeSignature?> GetByEmployeeId(Guid employeeId)
        {
            return await _context.Set<EmployeeSignature>()
                .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
        }

        public async Task<bool> ExistsByEmployeeId(Guid employeeId)
        {
            return await _context.Set<EmployeeSignature>()
                .AnyAsync(s => s.EmployeeId == employeeId);
        }
    }
}
