using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeServiceInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        
        [Required]
        public Guid BelongingUnitId { get; set; }
        public OrgUnit BelongingUnit { get; set; } = null!;
        
        public int Rank { get; set; }
        
        public int Ownership { get; set; }
        
        [Required]
        public Guid JobRoleId { get; set; }
        public Role JobRole { get; set; } = null!;
        
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
} 