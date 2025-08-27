using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class OrgUnit : BaseEntity, IActivable
    {
        public OrgUnitType Type { get; set; }
        public int Level;

        [Required]
        [StringLength(200)]
        public string OfficialName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AliasName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ShortName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string EnglishName { get; set; } = string.Empty;

        public string HierarchyPath { get; set; } = string.Empty;

        public long? OldUnitId { get; set; } // from old system
        public long? OldParentId { get; set; } // from old system
        public string? Description { get; set; }

        public long? ParentId { get; set; }
        public OrgUnit? Parent { get; set; }

        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        public ICollection<OrgUnit> Children { get; set; } = [];
        // Navigation properties for relationships
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = [];
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = [];
        public bool IsActive { get; set; } = true;
    }
}