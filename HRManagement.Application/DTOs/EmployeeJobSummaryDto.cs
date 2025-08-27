namespace HRManagement.Application.DTOs
{
    public class EmployeeJobSummaryDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long JobRoleId { get; set; }
        public required string JobRoleName { get; set; }
        public long? UnitId { get; set; }
        public string? UnitName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}