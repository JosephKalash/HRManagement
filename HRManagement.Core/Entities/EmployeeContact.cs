using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeContact : BaseEntity
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

        public virtual Employee Employee { get; set; }
    }
}