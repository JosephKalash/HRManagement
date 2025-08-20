using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{

    public class OrgUnit : BaseEntity
    {
        public OrgUnitType Type { get; set; }
        public int Level => (int)Type;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string HierarchyPath { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid? ParentId { get; set; }
        public OrgUnit? Parent { get; set; }
        public ICollection<OrgUnit> Children { get; set; } = [];

        public Guid? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = [];
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = [];
    }
}