namespace HRManagement.Core.Entities
{

    public abstract class AuditedEntity : BaseEntity
    {
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}