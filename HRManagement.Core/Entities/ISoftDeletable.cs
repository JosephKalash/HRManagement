namespace HRManagement.Core.Entities;

public interface IAuditSoftDelete
{
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
}