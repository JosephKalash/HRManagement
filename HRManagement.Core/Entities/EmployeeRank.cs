using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeRank : AuditedEntity, IActivable
    {
        [Required]
        public long EmployeeId { get; set; }
        
        [Required]
        public long RankId { get; set; }
        
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? EffectiveDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
        public virtual Rank Rank { get; set; } = null!;
    }
}