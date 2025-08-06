using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class EmployeeServiceInfoDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid BelongingUnitId { get; set; }
        public int Ownership { get; set; }
        public Guid JobRoleId { get; set; }
        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public decimal? BaseSalary { get; set; }
        public bool IsMilitaryCoach { get; set; }
        public bool IsDeductedMinistryVacancies { get; set; }
        public bool IsRetiredFederalMinistry { get; set; }
        public bool IsNationalService { get; set; }
        public int ProfessionalSupport { get; set; }
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
        public int Rank { get; set; }

        [Required]
        public int Ownership { get; set; }

        [Required]
        public Guid JobRoleId { get; set; }

        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public decimal? BaseSalary { get; set; }
        public bool IsMilitaryCoach { get; set; }
        public bool IsDeductedMinistryVacancies { get; set; }
        public bool IsRetiredFederalMinistry { get; set; }
        public bool IsNationalService { get; set; }
        public int ProfessionalSupport { get; set; } = 0;

        [Required]
        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateEmployeeServiceInfoDto
    {
        public Guid? BelongingUnitId { get; set; }
        public int? Rank { get; set; }
        public int? Ownership { get; set; }
        public Guid? JobRoleId { get; set; }
        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public decimal? BaseSalary { get; set; }
        public bool? IsMilitaryCoach { get; set; }
        public bool? IsDeductedMinistryVacancies { get; set; }
        public bool? IsRetiredFederalMinistry { get; set; }
        public bool? IsNationalService { get; set; }
        public int? ProfessionalSupport { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
} 