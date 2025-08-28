using HRManagement.Core.Entities;
using HRManagement.Core.Enums;

namespace HRManagement.Application.DTOs
{
    public class OrgUnitDto
    {
        public long Id { get; set; }
        public OrgUnitType Type { get; set; }
        public int Level { get; set; }
        public string OfficialName { get; set; } = string.Empty;
        public string AliasName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string HierarchyPath { get; set; } = string.Empty;
        public long? ParentId { get; set; }
        public long? DeputyUnitId { get; set; }
        public int? MOIUnitId { get; set; }
        public int? CenterCode { get; set; }
        public bool ExchangeActive { get; set; }
        public long? ManagerId { get; set; }
        public bool IsActive { get; set; }
        public List<OrgUnitDto> Children { get; set; } = new(); // For hierarchical representation
    }

    public class CreateOrgUnitDto
    {
        public OrgUnitType Type { get; set; }
        public int Level { get; set; }
        public string OfficialName { get; set; } = string.Empty;
        public string AliasName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string? HierarchyPath { get; set; }
        public long? ParentId { get; set; } // Optional parent unit
        public long? DeputyUnitId { get; set; }
        public int? MOIUnitId { get; set; }
        public int? CenterCode { get; set; }
        public bool ExchangeActive { get; set; } = true;
        public long? ManagerId { get; set; } // Optional manager
        public bool IsActive { get; set; } = true;
    }

    public class UpdateOrgUnitDto
    {
        public OrgUnitType? Type { get; set; }
        public int? Level { get; set; }
        public string? OfficialName { get; set; }
        public string? AliasName { get; set; }
        public string? ShortName { get; set; }
        public string? EnglishName { get; set; }
        public string? HierarchyPath { get; set; }
        public long? ParentId { get; set; } // Optional parent unit
        public long? DeputyUnitId { get; set; }
        public int? MOIUnitId { get; set; }
        public int? CenterCode { get; set; }
        public bool? ExchangeActive { get; set; }
        public long? ManagerId { get; set; } // Optional manager
        public bool? IsActive { get; set; }
    }

    public class OrgUnitHierarchyDto
    {
        public long Id { get; set; }
        public string OfficialName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public long? ManagerId { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public List<OrgUnitHierarchyDto> Children { get; set; } = []; // Recursive hierarchy
    }
}