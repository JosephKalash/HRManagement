namespace HRManagement.Application.DTOs
{
    public class EmployeeSignatureDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string SignatureName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? OriginalFileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
