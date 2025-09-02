using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeServiceInfo : BaseEntity, IActivable
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long BelongingUnitId { get; set; }
        public string? UnitNote { get; set; }
        public long? AttachedUnitId { get; set; } // Unit to which the employee is attached, not important in e-services but just as info-recorded field
        public string? AttachedUnitNote { get; set; }
        public DateTime? AttachedStartDate { get; set; }
        public DateTime? AttachedEndDate { get; set; }

        [Required]
        public long JobRoleId { get; set; }
        public DateTime? EndDate{ get; set; }
        public bool IsActive { get; set; } = true;

        //navigation
        public Employee Employee { get; set; } = null!;
        public OrgUnit BelongingUnit { get; set; } = null!;
        public Role JobRole { get; set; } = null!;
    }
}