namespace LexiFlow.API.DTOs.Kanji
{
    public class KanjiSearchRequestDto
    {
        public string Query { get; set; } = "";
        public string JLPTLevel { get; set; } = "";
        public int? Grade { get; set; }
        public int? StrokeCount { get; set; }
        public string Radical { get; set; } = "";
        public int MaxResults { get; set; } = 20;
    }
}
