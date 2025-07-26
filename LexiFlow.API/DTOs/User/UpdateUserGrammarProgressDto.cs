using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class UpdateUserGrammarProgressDto
    {
        public int UnderstandingLevel { get; set; }
        public int UsageLevel { get; set; }
        public DateTime? LastStudied { get; set; }
        public int StudyCount { get; set; }
        public float? TestScore { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string PersonalNotes { get; set; }

        [Required]
        public string RowVersionString { get; set; }
    }
}
