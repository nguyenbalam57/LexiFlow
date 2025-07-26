namespace LexiFlow.API.DTOs.User
{
    public class UserTechnicalTermDto
    {
        public int UserTermID { get; set; }
        public int UserID { get; set; }
        public int TermID { get; set; }
        public string Term { get; set; }
        public bool IsBookmarked { get; set; }
        public int UsageFrequency { get; set; }
        public DateTime? LastUsed { get; set; }
        public string PersonalExample { get; set; }
        public string WorkContext { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
