namespace LexiFlow.API.DTOs.User
{
    public class UserVocabularyProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int VocabularyID { get; set; }
        public string Term { get; set; }
        public int RecognitionLevel { get; set; }
        public int MemorizationLevel { get; set; }
        public DateTime? LastPracticed { get; set; }
        public int PracticeCount { get; set; }
        public int CorrectCount { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
