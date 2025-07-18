namespace LexiFlow.API.Models
{
    public class Vocabulary
    {
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public string? Kana { get; set; }
        public string? Romaji { get; set; }
        public string? Vietnamese { get; set; }
        public string? English { get; set; }
        public string? Example { get; set; }
        public string? Notes { get; set; }
        public string? Level { get; set; }
        public int? GroupID { get; set; }
        public string? PartOfSpeech { get; set; }
        public string? AudioFile { get; set; }
        public int CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
