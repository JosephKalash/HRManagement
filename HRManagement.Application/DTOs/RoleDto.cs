using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateRoleDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateRoleDto
    {
        [Required]
        public long Id { get; set; } // Role ID for identification

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; } // Indicates if the role is active
    }
}