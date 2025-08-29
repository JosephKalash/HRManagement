


using System.ComponentModel.DataAnnotations;
using HRManagement.Core.Entities;
namespace HRManagement.Application.DTOs;
public class EmployeeRankDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public long EmployeeId { get; set; }
    public long RankId { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public RankDto? Rank { get; set; }
}


public class CreateEmployeeRankDto
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    public long RankId { get; set; }

    public DateTime? EffectiveDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
}


public class UpdateEmployeeRankDto
{
    public bool IsActive { get; set; }
    public DateTime? EffectiveDate { get; set; }
}