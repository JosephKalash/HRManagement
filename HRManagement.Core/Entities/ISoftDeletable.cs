namespace HRManagement.Core.Entities;

public interface IAuditSoftDelete
{
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}