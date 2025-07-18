namespace LexiFlow.API.Models.DTOs
{
    public class CreateVocabularyDto
    {
        public string Japanese { get; set; }
        public string? Kana { get; set; }
        public string? Romaji { get; set; }
        public string? Vietnamese { get; set; }
        public string? English { get; set; }
        public string? Example { get; set; }
        public string? Notes { get; set; }
        public string? Level { get; set; }
        public string? PartOfSpeech { get; set; }
        public int? GroupID { get; set; }
    }
}
