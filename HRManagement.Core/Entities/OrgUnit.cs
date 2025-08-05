using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public enum OrgUnitType
    {
        LeaderOfficer = 1,
        ViceLeaderOfficer = 2,
        GeneralManagement = 3,
        Department = 4,
        Section = 5,
        Branch = 6
    }

    public class OrgUnit
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public OrgUnitType Type { get; set; }
        public int Level => (int)Type;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid? ParentId { get; set; }
        public OrgUnit? Parent { get; set; }
        public ICollection<OrgUnit> Children { get; set; } = new List<OrgUnit>();

        public Guid? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = new List<EmployeeServiceInfo>();
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = new List<EmployeeAssignment>();
    }
}