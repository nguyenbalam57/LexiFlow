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

    public class UpdateLearningProgressDto
    {
        [Required]
        public int VocabularyID { get; set; }
        public bool IsCorrect { get; set; }
        public int MemoryStrength { get; set; }
    }

    public class ReviewItemDto
    {
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public string Kana { get; set; }
        public string Vietnamese { get; set; }
        public string English { get; set; }
        public string Example { get; set; }
        public string Level { get; set; }
        public string PartOfSpeech { get; set; }
        public int MemoryStrength { get; set; }
        public DateTime? NextReviewDate { get; set; }
    }

    public class ReviewSessionRequestDto
    {
        public int Count { get; set; } = 20;
        public bool IncludeOverdue { get; set; } = true;
        public bool IncludeNew { get; set; } = true;
        public int? MaxMemoryStrength { get; set; }
        public string LevelFilter { get; set; }
    }

    public class ReviewSessionResultDto
    {
        public int TotalReviewed { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public List<ReviewItemResultDto> ItemResults { get; set; } = new List<ReviewItemResultDto>();
    }

    public class ReviewItemResultDto
    {
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public bool IsCorrect { get; set; }
        public int OldMemoryStrength { get; set; }
        public int NewMemoryStrength { get; set; }
        public DateTime? NextReviewDate { get; set; }
    }
}
