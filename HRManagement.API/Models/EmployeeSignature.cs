using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.Models
{
    public class CreateEmployeeSignatureRequest
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string SignatureName { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }

    public class UpdateEmployeeSignatureRequest
    {
        [StringLength(100)]
        public string? SignatureName { get; set; }
    }
}
