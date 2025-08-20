using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

namespace HRManagement.Application.DTOs
{
    public class OrgUnitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public OrgUnitType Type { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? ManagerId { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public List<OrgUnitDto> Children { get; set; } = new(); // For hierarchical representation
    }

    public class CreateOrgUnitDto
    {
        public string Name { get; set; } = string.Empty;
        public OrgUnitType Type { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; } // Optional parent unit
        public Guid? ManagerId { get; set; } // Optional manager
    }

    public class UpdateOrgUnitDto
    {
        public string? Name { get; set; }
        public OrgUnitType? Type { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; } // Optional parent unit
        public Guid? ManagerId { get; set; } // Optional manager
    }

    public class OrgUnitHierarchyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public List<OrgUnitHierarchyDto> Children { get; set; } = []; // Recursive hierarchy
    }
}