using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Learning
{
    public class LearningProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public string Kana { get; set; }
        public string Meaning { get; set; }
        public int StudyCount { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public DateTime? LastStudied { get; set; }
        public int MemoryStrength { get; set; }
        public DateTime? NextReviewDate { get; set; }
    }
}
