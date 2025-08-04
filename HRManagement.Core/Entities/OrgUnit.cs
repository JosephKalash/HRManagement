using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public enum OrgUnitType
    {
        General_Management,
        Management,
        Department,
        Section,
        Branch,
    }

    public class OrgUnit
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public OrgUnitType Type { get; set; }

        public Guid? ParentId { get; set; }
        public OrgUnit? Parent { get; set; }
        public ICollection<OrgUnit> Children { get; set; } = new List<OrgUnit>();
        // Level property based on OrgUnitType
        public int Level => Type switch
        {
            OrgUnitType.General_Management => 1,
            OrgUnitType.Management => 2,
            OrgUnitType.Department => 3,
            OrgUnitType.Section => 4,
            OrgUnitType.Branch => 5,
            _ => 0
        };
    }
}