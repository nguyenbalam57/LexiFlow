namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for Kanji in vocabulary
    /// </summary>
    public class KanjiVocabularyDto
    {
        public int KanjiId { get; set; }
        public string Character { get; set; }
        public string OnYomi { get; set; }
        public string KunYomi { get; set; }
        public string Meaning { get; set; }
        public int Position { get; set; }
        public string JLPTLevel { get; set; }
    }
}
