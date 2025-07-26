namespace LexiFlow.API.DTOs.User
{
    public class UserLearningProgressDto
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int VocabularyCount { get; set; }
        public int KanjiCount { get; set; }
        public int GrammarCount { get; set; }
        public int TechnicalTermCount { get; set; }
        public int TotalStudyTimeMinutes { get; set; }
        public DateTime LastActiveDate { get; set; }
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
