using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class EmployeeServiceInfo : BaseEntity, IActivable
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long BelongingUnitId { get; set; }
        public string? UnitNote{ get; set; }
        
        public long? AttachedUnitId { get; set; } // Unit to which the employee is attached, not important in e-services but just as info-recorded field
        public string? AttachedUnitNote { get; set; }
        public DateTime? AttachedStartDate { get; set; }
        public DateTime? AttachedEndDate { get; set; }

        public Ownership Ownership { get; set; } = Ownership.Local;

        [Required]
        public long JobRoleId { get; set; }

        public DateTime? HiringDate { get; set; }

        public int? GrantingAuthority { get; set; }

        public DateTime? LastPromotion { get; set; }

        public int? ContractDuration { get; set; }
        public ContractType ContractType { get; set; } = ContractType.Local;

        public int? ServiceDuration { get; set; }

        public decimal? BaseSalary { get; set; }

        public bool IsMilitaryTraining { get; set; } = false;
        public bool IsDeductedMinistryVacancies { get; set; } = false; 

        public bool IsRetiredFederalGov { get; set; } = false;

        public bool IsNationalService { get; set; } = false;

        public bool IsProfessionalSupportNeeded { get; set; } = false;

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        //navigation
        public Employee Employee { get; set; } = null!;
        public OrgUnit BelongingUnit { get; set; } = null!;
        public Role JobRole { get; set; } = null!;
    }
}