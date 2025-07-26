namespace LexiFlow.API.DTOs.Learning
{
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
}
