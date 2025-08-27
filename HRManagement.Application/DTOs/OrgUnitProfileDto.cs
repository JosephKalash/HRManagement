using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class OrgUnitProfileDto
    {
        public long Id { get; set; }
        public long OrgUnitId { get; set; }
        public string? Specialization { get; set; }
        public int AllowedForcesOff { get; set; }
        public string? UnitCodeExtra { get; set; }
        public string? Phone { get; set; }
        public int? Link { get; set; }
        public string? JobDescPath { get; set; }
        public string? Stamp { get; set; }
        public bool ManagerOffLeaderApprov { get; set; }
        public string? CostNumber { get; set; }
    }

    public class CreateOrgUnitProfileDto
    {
        [Required]
        public long OrgUnitId { get; set; }
        [StringLength(500)]
        public string? Specialization { get; set; }
        public int AllowedForcesOff { get; set; } = 100;
        [StringLength(20)]
        public string? UnitCodeExtra { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }
        public int? Link { get; set; }
        [StringLength(200)]
        public string? JobDescPath { get; set; }
        [StringLength(200)]
        public string? Stamp { get; set; }
        public bool ManagerOffLeaderApprov { get; set; } = false;
        [StringLength(20)]
        public string? CostNumber { get; set; }
    }

    public class UpdateOrgUnitProfileDto
    {
        public long? OrgUnitId { get; set; }
        [StringLength(500)]
        public string? Specialization { get; set; }
        public int? AllowedForcesOff { get; set; }
        [StringLength(20)]
        public string? UnitCodeExtra { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }
        public int? Link { get; set; }
        [StringLength(200)]
        public string? JobDescPath { get; set; }
        [StringLength(200)]
        public string? Stamp { get; set; }
        public bool? ManagerOffLeaderApprov { get; set; }
        [StringLength(20)]
        public string? CostNumber { get; set; }
    }
}


