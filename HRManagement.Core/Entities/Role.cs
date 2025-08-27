using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class Role : BaseEntity, IActivable
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        public long? OldListId { get; set; } // from old system

        // [StringLength(20)]
        // public string? Code { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        // public RoleLevel Level { get; set; } = RoleLevel.Employee; // Default to EntryLevel

        // Navigation properties
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = new List<EmployeeServiceInfo>();
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = new List<EmployeeAssignment>();
    }
}