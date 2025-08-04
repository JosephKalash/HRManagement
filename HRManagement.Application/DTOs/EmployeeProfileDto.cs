using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class EmployeeProfileDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int? Height { get; set; }
        public int BloodGroup { get; set; }
        public string? SkinColor { get; set; }
        public string? HairColor { get; set; }
        public string? EyeColor { get; set; }
        public string? DisabilityType { get; set; }
        public string? DistinctiveSigns { get; set; }
        public string Image { get; set; } = string.Empty;
        public string CurrentNationality { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty;
        public int? PreviousNationality { get; set; }
        public DateTime? IssueNationalityDate { get; set; }
        public int SocialCondition { get; set; }
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string? InsuranceNumber { get; set; }
    }

    public class CreateEmployeeProfileDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        public int? Height { get; set; }

        [Required]
        public int BloodGroup { get; set; }

        [StringLength(50)]
        public string? SkinColor { get; set; }

        [StringLength(50)]
        public string? HairColor { get; set; }

        [StringLength(50)]
        public string? EyeColor { get; set; }

        [StringLength(100)]
        public string? DisabilityType { get; set; }

        public string? DistinctiveSigns { get; set; }

        [Required]
        [StringLength(500)]
        public string Image { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CurrentNationality { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Religion { get; set; } = string.Empty;

        public int? PreviousNationality { get; set; }

        public DateTime? IssueNationalityDate { get; set; }

        [Required]
        public int SocialCondition { get; set; }

        [Required]
        [StringLength(200)]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }
    }

    public class UpdateEmployeeProfileDto
    {
        public int? Height { get; set; }
        public int? BloodGroup { get; set; }

        [StringLength(50)]
        public string? SkinColor { get; set; }

        [StringLength(50)]
        public string? HairColor { get; set; }

        [StringLength(50)]
        public string? EyeColor { get; set; }

        [StringLength(100)]
        public string? DisabilityType { get; set; }

        public string? DistinctiveSigns { get; set; }

        [StringLength(500)]
        public string? Image { get; set; }

        [StringLength(100)]
        public string? CurrentNationality { get; set; }

        [StringLength(100)]
        public string? Religion { get; set; }

        public int? PreviousNationality { get; set; }
        public DateTime? IssueNationalityDate { get; set; }
        public int? SocialCondition { get; set; }

        [StringLength(200)]
        public string? PlaceOfBirth { get; set; }

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }
    }
} 