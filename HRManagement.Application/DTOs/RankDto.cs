using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class RankDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? EnglishName { get; set; }
        public int Order { get; set; }
    }

    public class CreateRankDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? EnglishName { get; set; }
        [Required]
        public int Order { get; set; }
    }

    public class UpdateRankDto
    {
        [StringLength(100)]
        public string? Name { get; set; }
        public string? EnglishName { get; set; }
        public int? Order { get; set; }
    }
}



