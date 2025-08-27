using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeProfileRepository : IRepository<EmployeeProfile>
    {
        Task<EmployeeProfile?> GetByEmployeeId(long employeeId);
        Task UpdateEmployeeImage(long employeeId, string imagePath);
        Task<string?> GetEmployeeImagePath(long employeeId);
    }
}