using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class UpdateEmployeeSignatureDto
    {
        [StringLength(100)]
        public string? SignatureName { get; set; }
    }
}
