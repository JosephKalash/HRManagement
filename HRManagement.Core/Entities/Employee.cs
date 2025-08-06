using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{

    public class Employee
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int MilitaryNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string ArabicFirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ArabicMiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string ArabicLastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EnglishFirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? EnglishMiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string EnglishLastName { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public MilitaryRank Rank { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(50)]
        public string IdNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual EmployeeProfile? Profile { get; set; }
        public virtual ICollection<EmployeeServiceInfo> ServiceInfos { get; set; } = new List<EmployeeServiceInfo>();
        public virtual ICollection<EmployeeAssignment> Assignments { get; set; } = new List<EmployeeAssignment>();
        // public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}