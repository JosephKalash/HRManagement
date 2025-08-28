using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class ShortEmployeeDto
    {
        public long Id { get; set; }
        public int MilitaryNumber { get; set; }
        public string ArabicName { get; set; } = string.Empty;
        public string RankName { get; set; } = string.Empty;
        // public string? ImagePath { get; set; }
    }
    public class EmployeeDto
    {
        public long Id { get; set; }
        public int MilitaryNumber { get; set; }
        public string ArabicFirstName { get; set; } = string.Empty;
        public string? ArabicMiddleName { get; set; }
        public string ArabicLastName { get; set; } = string.Empty;
        public string RankName { get; set; } = string.Empty;
        public string EnglishFirstName { get; set; } = string.Empty;
        public string? EnglishMiddleName { get; set; }
        public string EnglishLastName { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateEmployeeDto
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
        public long RankId { get; set; }

        [Required]
        [StringLength(50)]
        public string IdNumber { get; set; } = string.Empty;
    }

    public class UpdateEmployeeDto
    {
        public int? MilitaryNumber { get; set; }

        [StringLength(100)]
        public string? ArabicFirstName { get; set; }

        [StringLength(100)]
        public string? ArabicMiddleName { get; set; }

        [StringLength(100)]
        public string? ArabicLastName { get; set; }

        [StringLength(100)]
        public string? EnglishFirstName { get; set; }

        [StringLength(100)]
        public string? EnglishMiddleName { get; set; }

        [StringLength(100)]
        public string? EnglishLastName { get; set; }

        [StringLength(50)]
        public string? IdNumber { get; set; }
        public long? RankId { get; set; }

        public bool? IsActive { get; set; }
    }
}