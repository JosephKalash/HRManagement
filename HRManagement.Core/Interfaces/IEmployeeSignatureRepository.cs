using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeSignatureRepository : IRepository<EmployeeSignature>
    {
        Task<EmployeeSignature?> GetByEmployeeIdAsync(Guid employeeId);
        Task<bool> ExistsByEmployeeIdAsync(Guid employeeId);
    }
}
