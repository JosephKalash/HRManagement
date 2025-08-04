using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeAssignment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;
        
        [Required]
        public Guid AssignedUnitId { get; set; }
        public OrgUnit AssignedUnit { get; set; } = null!;
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        public Guid JobRoleId { get; set; }
        public Role JobRole { get; set; } = null!;
        
        [Required]
        public DateTime HiringDate { get; set; }
        
        public int GrantingAuthority { get; set; }
        
        public DateTime LastPromotion { get; set; }
        
        public int? ContractDuration { get; set; }
        
        public int? ServiceDuration { get; set; }
        
        public int AssignmentType { get; set; }
    }
} 