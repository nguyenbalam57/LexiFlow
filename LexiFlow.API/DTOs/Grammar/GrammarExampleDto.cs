namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarExampleDto
    {
        public int ExampleID { get; set; }
        public string JapaneseSentence { get; set; }
        public string Romaji { get; set; }
        public string VietnameseTranslation { get; set; }
        public string EnglishTranslation { get; set; }
        public string Context { get; set; }
        public string AudioFile { get; set; }
    }
}
