using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class PerformanceReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid EmployeeId { get; set; }
        
        [Required]
        public DateTime ReviewDate { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ReviewerName { get; set; } = string.Empty;
        
        [Range(1, 5)]
        public int OverallRating { get; set; }
        
        [StringLength(1000)]
        public string? Strengths { get; set; }
        
        [StringLength(1000)]
        public string? AreasForImprovement { get; set; }
        
        [StringLength(1000)]
        public string? Goals { get; set; }
        
        [StringLength(1000)]
        public string? Comments { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property
        public virtual Employee Employee { get; set; } = null!;
    }
} 