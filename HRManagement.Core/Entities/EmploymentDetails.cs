using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class EmploymentDetails : BaseEntity
    {
        public long EmployeeId { get; set; }

        public DateTime? HiringDate { get; set; }

        public int? GrantingAuthority { get; set; }

        public Ownership Ownership { get; set; } = Ownership.Local;

        public DateTime? LastPromotion { get; set; }

        public int? ContractDuration { get; set; }

        public ContractType ContractType { get; set; } = ContractType.Local;

        public int? ServiceDuration { get; set; }

        public int? BaseSalary { get; set; }

        public bool IsMilitaryTraining { get; set; } = false;

        public bool IsDeductedMinistryVacancies { get; set; } = false;

        public bool IsRetiredFederalGov { get; set; } = false;

        public bool IsNationalService { get; set; } = false;

        public bool IsProfessionalSupportNeeded { get; set; } = false;

        public DateTime? EndDate { get; set; }

        // Navigation property 
        public Employee? Employee { get; set; }  
    }
}
