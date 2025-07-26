namespace LexiFlow.API.DTOs.Kanji
{
    public class VocabularyReferenceDto
    {
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public string Kana { get; set; }
        public string Meaning { get; set; }
        public string Position { get; set; }
    }
}
