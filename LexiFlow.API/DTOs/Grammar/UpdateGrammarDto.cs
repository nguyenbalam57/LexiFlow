namespace LexiFlow.API.DTOs.Grammar
{
    public class UpdateGrammarDto : CreateGrammarDto
    {
        public string RowVersionString { get; set; }
    }
}
