namespace LexiFlow.API.DTOs.User
{
    public class UserKanjiProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int KanjiID { get; set; }
        public string Character { get; set; }
        public int RecognitionLevel { get; set; }
        public int WritingLevel { get; set; }
        public DateTime? LastPracticed { get; set; }
        public int PracticeCount { get; set; }
        public int CorrectCount { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
