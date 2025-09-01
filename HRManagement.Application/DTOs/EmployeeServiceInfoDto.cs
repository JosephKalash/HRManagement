using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

//todo
namespace HRManagement.Application.DTOs
{
    public class EmployeeServiceInfoDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long BelongingUnitId { get; set; }
        public Ownership Ownership { get; set; }
        public long JobRoleId { get; set; }
        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public decimal? BaseSalary { get; set; }
        public bool IsMilitaryCoach { get; set; } = false;
        public ContractType ContractType { get; set; }

        public bool IsDeductedMinistryVacancies { get; set; } = false;
        public bool IsRetiredFederalMinistry { get; set; } = false;
        public bool IsNationalService { get; set; } = false;
        public bool ProfessionalSupportCourse { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateEmployeeServiceInfoDto
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long BelongingUnitId { get; set; }


        [Required]
        public Ownership Ownership { get; set; }

        [Required]
        public long JobRoleId { get; set; }

        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public decimal? BaseSalary { get; set; }
        public bool IsMilitaryCoach { get; set; } = false;
        public bool IsDeductedMinistryVacancies { get; set; } = false;
        public bool IsRetiredFederalMinistry { get; set; } = false;
        public bool IsNationalService { get; set; } = false;
        public ContractType ContractType { get; set; } = ContractType.Local;

        public bool ProfessionalSupportCourse { get; set; } = false;

        [Required]
        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateEmployeeServiceInfoDto
    {
        public long? BelongingUnitId { get; set; }
        public Ownership? Ownership { get; set; }
        public long? JobRoleId { get; set; }
        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public decimal? BaseSalary { get; set; }
        public ContractType? ContractType { get; set; }

        public bool? IsMilitaryCoach { get; set; }
        public bool? IsDeductedMinistryVacancies { get; set; }
        public bool? IsRetiredFederalMinistry { get; set; }
        public bool? IsNationalService { get; set; }
        public bool? ProfessionalSupportCourse { get; set; }
        public bool? ProfessionalSupport { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}