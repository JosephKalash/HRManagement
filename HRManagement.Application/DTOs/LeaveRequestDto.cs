using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Entities;

namespace HRManagement.Application.DTOs
{
    public class LeaveRequestDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string? Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public string? ManagerComments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public EmployeeDto? Employee { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public LeaveType LeaveType { get; set; }

        [StringLength(500)]
        public string? Reason { get; set; }
    }

    public class UpdateLeaveRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveType? LeaveType { get; set; }
        public string? Reason { get; set; }
        public LeaveStatus? Status { get; set; }
        public string? ManagerComments { get; set; }
    }
} 