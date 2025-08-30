using HRManagement.Core.enums;

namespace HRManagement.Core.Entities;

public class EmployeeCard
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }

    public long RankId { get; set; }
    public long JobRoleId { get; set; }
    public long EmployeeSignatureId { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public CardStatus Status { get; set; }
    public CardIssueReason? IssueReason { get; set; }
    public bool IsPrinted { get; set; }

    public int IssuedByHRManagerId { get; set; }
    public int HRManagerSignatureId { get; set; }

    // Navigation properties
    public virtual Employee? Employee { get; set; }
    public virtual Rank? Rank { get; set; }
    public virtual Role? JobRole { get; set; }
    public virtual EmployeeSignature? EmployeeSignature { get; set; }
    public virtual Employee? IssuedByHRManager { get; set; }
    public virtual EmployeeSignature? HRManagerSignature { get; set; }
}