
namespace HRManagement.Core.Entities
{

    public abstract class AuditedEntity : BaseEntity
    {
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}