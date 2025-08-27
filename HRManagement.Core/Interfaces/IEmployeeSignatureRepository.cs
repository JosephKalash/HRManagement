using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeSignatureRepository : IRepository<EmployeeSignature>
    {
        Task<EmployeeSignature?> GetByEmployeeId(long employeeId);
        Task<bool> ExistsByEmployeeId(long employeeId);
    }
}
