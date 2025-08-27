using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class EmployeeServiceInfo : BaseEntity, IActivable
    {
        [Required]
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public long BelongingUnitId { get; set; }
        public OrgUnit BelongingUnit { get; set; } = null!;

        public Ownership Ownership { get; set; }

        [Required]
        public long JobRoleId { get; set; }
        public Role JobRole { get; set; } = null!;

        public DateTime? HiringDate { get; set; }

        public int? GrantingAuthority { get; set; }

        public DateTime? LastPromotion { get; set; }

        public int? ContractDuration { get; set; }
        public ContractType ContractType { get; set; } = ContractType.Local;

        public int? ServiceDuration { get; set; }

        public decimal? BaseSalary { get; set; }

        public bool IsMilitaryCoach { get; set; } = false;

        public bool IsDeductedMinistryVacancies { get; set; } = false;

        public bool IsRetiredFederalMinistry { get; set; } = false;

        public bool IsNationalService { get; set; } = false;

        public bool ProfessionalSupport { get; set; } = false;

        [Required]
        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}