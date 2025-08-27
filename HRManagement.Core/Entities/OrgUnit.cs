using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

//-- SELECT centerId, center, priority_order, Center_Level, center_alias, new_name, fullName, enCenter, alternative_center, moi_unitid, center_code, exchange_active, res_ecardno, ParentCenter FROM hr_external2.center where Center != 'test' and Center != '17410'and Display = 1 order by priority_order;
// main: centerId(as oldId), center(offical name), priority(map it to type), center_alias(alias), new_name(short name), Display(is_active), fullName(hierarchy path)
// enCenter, alternative_center (dupety), update_record_time, moi_unitid, center_code, exchange_active
namespace HRManagement.Core.Entities
{
    public class OrgUnit : BaseEntity, IActivable
    {
        public OrgUnitType Type { get; set; }
        public int Level;

        [Required]
        [StringLength(200)]
        public string OfficialName { get; set; } = string.Empty; // center from old db

        [Required]
        [StringLength(200)]
        public string AliasName { get; set; } = string.Empty;// center_alias from old db

        [Required]
        [StringLength(200)]
        public string ShortName { get; set; } = string.Empty;// new_name

        [Required]
        [StringLength(200)]
        public string EnglishName { get; set; } = string.Empty;
        public string HierarchyPath { get; set; } = string.Empty;
        public long? ParentId { get; set; }
        public long? DeputyUnitId { get; set; }
        public int? MOIUnitId { get; set; }
        public int? CenterCode { get; set; }
        public bool ExchangeActive { get; set; } = true;
        public long? ManagerId { get; set; }
        public bool IsActive { get; set; } = true;

        public OrgUnit? Parent { get; set; }
        public OrgUnit? DeputyUnit { get; set; }
        public Employee? Manager { get; set; }
        public OrgUnitProfile? Profile { get; set; }
        public ICollection<OrgUnit> Children { get; set; } = [];
        public virtual ICollection<EmployeeServiceInfo> EmployeeServiceInfos { get; set; } = [];
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; } = [];
    }
}