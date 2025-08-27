using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
// using HRManagement.Core.Enums;

namespace HRManagement.Application.DTOs
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty; // System identification
        public string? Description { get; set; }
        public RoleLevel Level { get; set; } // Organizational role level
        public int LevelNumber { get; set; } // Organizational role level
        public bool IsActive { get; set; } // Indicates if the role is active
        public bool IsSystemRole { get; set; } = false; // Indicates if the role is a system role
    }

    public class CreateRoleDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // System identification

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public RoleLevel Level { get; set; } // Organizational role level

        public bool IsSystemRole { get; set; } = false; // Indicates if the role is a system role
    }

    public class UpdateRoleDto
    {
        [Required]
        public long Id { get; set; } // Role ID for identification

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // System identification

        [StringLength(500)]
        public string? Description { get; set; }

        public RoleLevel? Level { get; set; } // Organizational role level

        public bool? IsActive { get; set; } // Indicates if the role is active
    }
}