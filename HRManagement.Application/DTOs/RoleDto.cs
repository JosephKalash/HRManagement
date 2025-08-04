using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public int? Level { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CreateRoleDto
    {
        public int? Level { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public class UpdateRoleDto
    {
        public int? Level { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
} 