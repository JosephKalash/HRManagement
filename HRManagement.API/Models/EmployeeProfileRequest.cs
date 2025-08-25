
using System.ComponentModel.DataAnnotations;
using HRManagement.Core.enums;
using HRManagement.Core.Enums;

namespace HRManagement.API.Models
{

    public class CreateEmployeeProfileRequest
    {
        [Required]
        public Guid EmployeeId { get; set; }

        public int? Height { get; set; }

        public BloodGroup? BloodGroup { get; set; }

        [StringLength(50)]
        public string? SkinColor { get; set; }
        public Gender? Gender { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

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

        public Religions Religion { get; set; } = Religions.Islam;

        public string? PreviousNationality { get; set; }

        public DateTime? IssueNationalityDate { get; set; }

        [Required]
        public SocialCondition? SocialCondition { get; set; }

        [Required]
        [StringLength(200)]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class UpdateEmployeeProfileRequest
    {
        public IFormFile? Image { get; set; }
        public int? Height { get; set; }
        public BloodGroup? BloodGroup { get; set; }
        public Gender? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public string? SkinColor { get; set; }

        [StringLength(50)]
        public string? HairColor { get; set; }

        [StringLength(50)]
        public string? EyeColor { get; set; }

        [StringLength(100)]
        public string? DisabilityType { get; set; }

        public string? DistinctiveSigns { get; set; }


        [StringLength(100)]
        public string? CurrentNationality { get; set; }

        public Religions? Religion { get; set; }

        public string? PreviousNationality { get; set; }
        public DateTime? IssueNationalityDate { get; set; }
        public SocialCondition? SocialCondition { get; set; }

        [StringLength(200)]
        public string? PlaceOfBirth { get; set; }

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }
    }
}