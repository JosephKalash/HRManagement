using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class EmployeeProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int? Height { get; set; }

        public BloodGroup? BloodGroup { get; set; }

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
        [StringLength(100)]
        public string CurrentNationality { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public Religions Religion { get; set; } = Religions.Islam;

        public string? PreviousNationality { get; set; }

        public DateTime? IssueNationalityDate { get; set; }

        public SocialCondition SocialCondition { get; set; }

        [Required]
        [StringLength(200)]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }

        [StringLength(500)]
        public string? ImagePath { get; set; } = string.Empty; // Store the file path of the uploaded image
    }
}