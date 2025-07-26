using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class UpdateUserTechnicalTermDto
    {
        public bool IsBookmarked { get; set; }
        public int UsageFrequency { get; set; }
        public DateTime? LastUsed { get; set; }
        public string PersonalExample { get; set; }
        public string WorkContext { get; set; }

        [Required]
        public string RowVersionString { get; set; }
    }
}
