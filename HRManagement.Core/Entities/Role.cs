using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class Role : AuditedEntity, IActivable
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(20)]
        public string? Code { get; set; }
        public required string OldListId { get; set; } // from old system

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        // public RoleLevel Level { get; set; } = RoleLevel.Employee; // Default to EntryLevel

        // Navigation properties
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = new List<EmployeeServiceInfo>();
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = new List<EmployeeAssignment>();
    }
}