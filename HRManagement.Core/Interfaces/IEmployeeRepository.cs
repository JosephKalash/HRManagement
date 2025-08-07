using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetByMilitaryNumberAsync(int militaryNumber);
        Task<Employee?> GetByIdNumberAsync(string idNumber);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
        Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);
        Task<Employee?> GetEmployeeWithAllDetailsAsync(Guid id);
    }
} 