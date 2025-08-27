using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class CreateEmployeeSignatureDto
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string SignatureName { get; set; } = string.Empty;
    }
}
