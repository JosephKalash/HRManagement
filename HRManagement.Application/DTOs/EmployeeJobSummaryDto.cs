namespace HRManagement.Application.DTOs
{
    public class EmployeeJobSummaryDto
    {
        public Guid Id { get; set; }
        public Guid JobRoleId { get; set; }
        public required string JobRoleName { get; set; }
        public Guid? UnitId { get; set; }
        public string? UnitName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}