using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetByMilitaryNumber(int militaryNumber);
        Task<Employee?> GetByIdNumber(string idNumber);
        Task<IEnumerable<Employee>> GetActiveEmployees();
        Task<IEnumerable<Employee>> SearchEmployees(string searchTerm);
        Task<Employee?> GetEmployeeWithAllDetails(Guid id);
    }
}