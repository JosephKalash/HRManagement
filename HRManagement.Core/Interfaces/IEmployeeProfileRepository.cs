using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeProfileRepository : IRepository<EmployeeProfile>
    {
        Task<EmployeeProfile?> GetByEmployeeIdAsync(Guid employeeId);
        Task UpdateEmployeeImageAsync(Guid employeeId, string imagePath);
    }
}