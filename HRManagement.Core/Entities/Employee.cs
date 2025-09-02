using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Core.Entities
{

    public class Employee : AuditedEntity, IActivable, IAuditSoftDelete
    {

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

        [Required]
        [StringLength(50)]
        public string IdNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        // Navigation properties
        public virtual ICollection<EmployeeRank> EmployeeRanks { get; set; } = [];
        // public virtual EmployeeRank? CurrentRank => EmployeeRanks
        //     .Where(er => er.IsActive && (er.EndDate == null || er.EndDate > DateTime.UtcNow))
        //     .FirstOrDefault();
        public virtual EmployeeProfile? Profile { get; set; }
        public virtual EmployeeContact? Contact { get; set; }
        public virtual EmployeeSignature? Signature { get; set; }
        public virtual ICollection<EmployeeServiceInfo> ServiceInfos { get; set; } = [];
        public virtual ICollection<EmployeeAssignment> Assignments { get; set; } = [];
        public virtual EmploymentDetails? EmploymentDetails { get; set; }
    }
}