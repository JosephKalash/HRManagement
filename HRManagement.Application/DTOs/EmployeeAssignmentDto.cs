using System.ComponentModel.DataAnnotations;
using HRManagement.Core.enums;

namespace HRManagement.Application.DTOs
{
    public class EmployeeAssignmentDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public long AssignedUnitId { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public long JobRoleId { get; set; }
        public DateTime HiringDate { get; set; }
        public int GrantingAuthority { get; set; }
        public DateTime LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
    }

    public class CreateEmployeeAssignmentDto
    {
        [Required]
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; } = true;
        [Required]
        public long AssignedUnitId { get; set; }
        [Required]
        public long JobRoleId { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        [Required]
        public AssignmentType AssignmentType { get; set; } = AssignmentType.Temporary;
        [Required]
        public DateTime AssignmentDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateEmployeeAssignmentDto
    {
        public bool? IsActive { get; set; }
        public long? AssignedUnitId { get; set; }
        public long? JobRoleId { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public DateTime? HiringDate { get; set; }
        public int? GrantingAuthority { get; set; }
        public DateTime? LastPromotion { get; set; }
        public int? ContractDuration { get; set; }
        public int? ServiceDuration { get; set; }
        public AssignmentType? AssignmentType { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}