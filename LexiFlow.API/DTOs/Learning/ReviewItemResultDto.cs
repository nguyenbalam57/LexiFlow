namespace LexiFlow.API.DTOs.Learning
{
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
