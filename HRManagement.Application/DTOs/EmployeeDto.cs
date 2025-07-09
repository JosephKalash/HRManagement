using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [StringLength(100)]
        public string? Position { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        public decimal Salary { get; set; }
    }

    public class UpdateEmployeeDto
    {
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(100)]
        public string? Position { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        public decimal? Salary { get; set; }

        public bool? IsActive { get; set; }
    }
} 