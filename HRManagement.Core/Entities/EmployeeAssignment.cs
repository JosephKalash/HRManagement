using System.ComponentModel.DataAnnotations;
using HRManagement.Core.enums;

namespace HRManagement.Core.Entities
{
    public class EmployeeAssignment : AuditedEntity, IActivable
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long AssignedUnitId { get; set; }

        [Required]
        public long JobRoleId { get; set; }

        public DateTime? AssignmentDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ServiceDuration { get; set; }

        public AssignmentType AssignmentType { get; set; } = AssignmentType.Temporary;

        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;

        //Navigation properties
        public virtual Employee? CreatedByUser { get; set; }
        public virtual Employee? UpdatedByUser { get; set; }
        public OrgUnit AssignedUnit { get; set; } = null!;
        public Role JobRole { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
    }
}