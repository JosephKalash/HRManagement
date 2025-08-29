using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class Rank : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        public string? EnglishName { get; set; }
        public int Order { get; set; } = 1;

        //navigations
        public virtual ICollection<EmployeeRank> EmployeeRanks { get; set; } = [];
    }
}