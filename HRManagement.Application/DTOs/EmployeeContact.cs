using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class EmployeeContactDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? SecondMobileNumber { get; set; }
        public string? Address { get; set; }
    }
    public class CreateEmployeeContactDto
    {
        public long EmployeeId { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public required string MobileNumber { get; set; }
        [Phone]
        public string? SecondMobileNumber { get; set; }
        [StringLength(300)]
        public string? Address { get; set; }
    }
    public class UpdateEmployeeContactDto
    {
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? SecondMobileNumber { get; set; }
        public string? Address { get; set; }
    }
}