namespace LexiFlow.API.DTOs.User
{
    public class UserGrammarProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int GrammarID { get; set; }
        public string Pattern { get; set; }
        public int UnderstandingLevel { get; set; }
        public int UsageLevel { get; set; }
        public DateTime? LastStudied { get; set; }
        public int StudyCount { get; set; }
        public float? TestScore { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string PersonalNotes { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
