using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public enum LeaveType
    {
        Annual,
        Sick,
        Personal,
        Maternity,
        Paternity,
        Unpaid
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public class LeaveRequest
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public LeaveType LeaveType { get; set; }
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        
        [StringLength(500)]
        public string? ManagerComments { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property
        public virtual Employee Employee { get; set; } = null!;
    }
} 