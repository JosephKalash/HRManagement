using HRManagement.Core.Entities;

namespace HRManagement.Core.Interfaces
{
    public interface IEmployeeSignatureRepository : IRepository<EmployeeSignature>
    {
        Task<EmployeeSignature?> GetByEmployeeId(Guid employeeId);
        Task<bool> ExistsByEmployeeId(Guid employeeId);
    }
}
