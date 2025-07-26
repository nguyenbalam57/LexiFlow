namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarSearchRequestDto
    {
        public string Query { get; set; } = "";
        public string JLPTLevel { get; set; } = "";
        public int? CategoryID { get; set; }
        public string Pattern { get; set; } = "";
        public int MaxResults { get; set; } = 20;
    }
}
