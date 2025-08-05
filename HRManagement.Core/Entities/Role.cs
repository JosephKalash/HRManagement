using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = new List<EmployeeServiceInfo>();
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = new List<EmployeeAssignment>();
    }
}

namespace HRManagement.Core.Enums
{
    public enum RoleLevel
    {
        EntryLevel = 1, // Basic roles
        Employee = 2,   // Intermediate roles
        Assistent = 3, // Senior roles
        Manager = 4,  // Executive roles
        Admin = 5       // Administrative roles
    }
}