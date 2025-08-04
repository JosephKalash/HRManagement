using HRManagement.Core.Entities;

namespace HRManagement.Application.DTOs
{
    public class OrgUnitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public OrgUnitType Type { get; set; }
        public Guid? ParentId { get; set; }
        public List<OrgUnitDto> Children { get; set; } = new();
    }
} 