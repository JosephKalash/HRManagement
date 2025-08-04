using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class EmployeeAssignmentDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public Guid AssignedUnitId { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid JobRoleId { get; set; }
        public DateTime HiringDate { get; set; }
        public int GrantingAuthority { get; set; }
        public DateTime LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public int AssignmentType { get; set; }
    }

    public class CreateEmployeeAssignmentDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public Guid AssignedUnitId { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Guid JobRoleId { get; set; }

        [Required]
        public DateTime HiringDate { get; set; }

        [Required]
        public int GrantingAuthority { get; set; }

        [Required]
        public DateTime LastPromotion { get; set; }

        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }

        [Required]
        public int AssignmentType { get; set; }
    }

    public class UpdateEmployeeAssignmentDto
    {
        public bool? IsActive { get; set; }
        public Guid? AssignedUnitId { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? JobRoleId { get; set; }
        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public int? AssignmentType { get; set; }
    }
} 