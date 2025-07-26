namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int GrammarID { get; set; }
        public string GrammarPoint { get; set; }
        public int UnderstandingLevel { get; set; }
        public int UsageLevel { get; set; }
        public DateTime? LastStudied { get; set; }
        public int StudyCount { get; set; }
        public float? TestScore { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string PersonalNotes { get; set; }
    }
}
