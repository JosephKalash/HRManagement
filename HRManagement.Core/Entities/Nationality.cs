namespace HRManagement.Core.Entities;

public class Nationality
{
    public long Id { get; set; }
    public string? NameEn { get; set; } 
    public required string NameAr { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Navigation
    public ICollection<EmployeeProfile> CurrentEmployees { get; set; } = [];
    public ICollection<EmployeeProfile> PreviousEmployees { get; set; } = [];
}
