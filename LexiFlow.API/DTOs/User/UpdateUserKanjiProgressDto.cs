using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class UpdateUserKanjiProgressDto
    {
        public int RecognitionLevel { get; set; }
        public int WritingLevel { get; set; }
        public DateTime? LastPracticed { get; set; }
        public int PracticeCount { get; set; }
        public int CorrectCount { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string Notes { get; set; }

        [Required]
        public string RowVersionString { get; set; }
    }
}
