using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Application.DTOs
{
    public class EmployeeServiceInfoDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid BelongingUnitId { get; set; }
        public Ownership Ownership { get; set; }
        public Guid JobRoleId { get; set; }
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
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid BelongingUnitId { get; set; }


        [Required]
        public Ownership Ownership { get; set; }

        [Required]
        public Guid JobRoleId { get; set; }

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
        public Guid? BelongingUnitId { get; set; }
        public Ownership? Ownership { get; set; }
        public Guid? JobRoleId { get; set; }
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