namespace HRManagement.Application.DTOs
{
    public class CurrentEmployeeSummaryDto
    {
        public EmployeeDto Employee { get; set; } = null!;
        public string? ProfileImagePath { get; set; }
        public List<EmployeeJobSummaryDto> JobSummary { get; set; } = new();
    }
}
