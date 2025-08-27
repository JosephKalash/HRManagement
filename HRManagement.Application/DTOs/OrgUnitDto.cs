using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

namespace HRManagement.Application.DTOs
{
    public class OrgUnitDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public OrgUnitType Type { get; set; }
        public string? Description { get; set; }
        public long? ParentId { get; set; }
        public long? ManagerId { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public List<OrgUnitDto> Children { get; set; } = new(); // For hierarchical representation
    }

    public class CreateOrgUnitDto
    {
        public string Name { get; set; } = string.Empty;
        public OrgUnitType Type { get; set; }
        public string? Description { get; set; }
        public long? ParentId { get; set; } // Optional parent unit
        public long? ManagerId { get; set; } // Optional manager
    }

    public class UpdateOrgUnitDto
    {
        public string? Name { get; set; }
        public OrgUnitType? Type { get; set; }
        public string? Description { get; set; }
        public long? ParentId { get; set; } // Optional parent unit
        public long? ManagerId { get; set; } // Optional manager
    }

    public class OrgUnitHierarchyDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? ManagerId { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public List<OrgUnitHierarchyDto> Children { get; set; } = []; // Recursive hierarchy
    }
}