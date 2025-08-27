using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class PerformanceReviewDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int OverallRating { get; set; }
        public string? Strengths { get; set; }
        public string? AreasForImprovement { get; set; }
        public string? Goals { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreatePerformanceReviewDto
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        [Required]
        [StringLength(100)]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
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
    }

    public class UpdatePerformanceReviewDto
    {
        public DateTime? ReviewDate { get; set; }

        [StringLength(100)]
        public string? ReviewerName { get; set; }

        [Range(1, 5)]
        public int? OverallRating { get; set; }

        [StringLength(1000)]
        public string? Strengths { get; set; }

        [StringLength(1000)]
        public string? AreasForImprovement { get; set; }

        [StringLength(1000)]
        public string? Goals { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }
    }
}