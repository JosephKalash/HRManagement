using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeProfileRepository : IRepository<EmployeeProfile>
    {
        Task<EmployeeProfile?> GetByEmployeeId(Guid employeeId);
        Task UpdateEmployeeImage(Guid employeeId, string imagePath);
        Task<string?> GetEmployeeImagePath(Guid employeeId);
    }
}