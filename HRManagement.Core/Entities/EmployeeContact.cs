using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeContact : BaseEntity
    {
        public long EmployeeId { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone]
        public required string MobileNumber { get; set; }
        [Phone]
        public string? SecondMobileNumber { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
        [Phone]
        public string? SecondPhoneNumber { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}