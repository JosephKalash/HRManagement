using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.DTOs
{
    public class EmployeeContactDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? SecondMobileNumber { get; set; }
        public string? Address { get; set; }
    }
    public class CreateEmployeeContactDto
    {
        public Guid EmployeeId { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
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