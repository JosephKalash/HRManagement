using System.ComponentModel.DataAnnotations;
using HRManagement.Core.enums;
using HRManagement.Core.Enums;

namespace HRManagement.Core.Entities
{
    public class EmployeeProfile : BaseEntity
    {
        [Required]
        public long EmployeeId { get; set; }
        public Gender Gender { get; set; } = Gender.Male;

        public DateTime? DateOfBirth { get; set; }

        public int? Height { get; set; }

        public BloodGroup? BloodGroup { get; set; }

        [StringLength(50)]
        public string? SkinColor { get; set; }

        [StringLength(50)]
        public string? HairColor { get; set; }

        [StringLength(50)]
        public string? EyeColor { get; set; }

        [StringLength(100)]
        public int? DisabilityType { get; set; }

        public string? DistinctiveSigns { get; set; }

        [Required]
        [StringLength(100)]
        public long? NationalityId { get; set; }

        [Required]
        [StringLength(100)]
        public Religions Religion { get; set; } = Religions.Islam;

        public long? PreviousNationalityId { get; set; }

        public DateTime? IssueNationalityDate { get; set; }

        public SocialCondition SocialCondition { get; set; }

        [Required]
        [StringLength(200)]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }

        [StringLength(500)]
        public string? ImagePath { get; set; } = string.Empty; // Store the file path of the uploaded image
        public string? MotherName { get; set; }
        public string? NickName { get; set; }

        // navigation
        public Employee Employee { get; set; } = null!;
        public Nationality? Nationality { get; set; }
        public Nationality? PreviousNationality { get; set; }
    }
}