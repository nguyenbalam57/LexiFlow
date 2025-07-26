using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarDto
    {
        public int Id { get; set; }
        public string Pattern { get; set; }
        public string LanguageCode { get; set; }
        public string Reading { get; set; }
        public string Level { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int DifficultyLevel { get; set; }
        public string Notes { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string RowVersionString { get; set; }
        public List<GrammarDefinitionDto> Definitions { get; set; } = new List<GrammarDefinitionDto>();
        public List<GrammarExampleDto> Examples { get; set; } = new List<GrammarExampleDto>();
        public List<GrammarTranslationDto> Translations { get; set; } = new List<GrammarTranslationDto>();
    }

}
