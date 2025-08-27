using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeSignature : BaseEntity, IActivable
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string SignatureName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(255)]
        public string? OriginalFileName { get; set; }

        // Navigation property
        public virtual Employee Employee { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
