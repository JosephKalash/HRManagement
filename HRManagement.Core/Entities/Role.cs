using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // For system identification (e.g., "ADMIN", "HR_MANAGER")

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public RoleLevel Level { get; set; } = RoleLevel.Employee; // Default to EntryLevel
        public bool IsSystemRole { get; set; } = false; // Cannot be deleted/modified

        // Hierarchy (optional - if roles have hierarchy)
        // public Guid? ParentRoleId { get; set; }
        // public Role? ParentRole { get; set; }
        // public ICollection<Role> ChildRoles { get; set; } = [];

        // public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = new List<EmployeeServiceInfo>();
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = new List<EmployeeAssignment>();
    }
}