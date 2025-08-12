using System.ComponentModel.DataAnnotations;
using HRManagement.Core.enums;

namespace HRManagement.Core.Entities
{
    public class EmployeeAssignment : BaseEntity
    {
        [Required]
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        public Guid AssignedUnitId { get; set; }
        public OrgUnit AssignedUnit { get; set; } = null!;


        [Required]
        public Guid JobRoleId { get; set; }
        public Role JobRole { get; set; } = null!;

        [Required]
        public DateTime AssignmentDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public DateTime HiringDate { get; set; }

        public int GrantingAuthority { get; set; }

        public DateTime LastPromotion { get; set; }

        public int? ContractDuration { get; set; }

        public int? ServiceDuration { get; set; }

        public AssignmentType AssignmentType { get; set; } = AssignmentType.Temporary;
        public string? Description { get; set; }
        public string? Name { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public virtual Employee? CreatedByUser { get; set; }
        public virtual Employee? UpdatedByUser { get; set; }
    }
}