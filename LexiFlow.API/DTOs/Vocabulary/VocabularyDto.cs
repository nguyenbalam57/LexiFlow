namespace LexiFlow.API.DTOs.Vocabulary
{
    public class VocabularyDto
    {
        public int Id { get; set; }
        public string Term { get; set; }
        public string LanguageCode { get; set; }
        public string Reading { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int DifficultyLevel { get; set; }
        public string Notes { get; set; }
        public string Tags { get; set; }
        public string AudioFile { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string RowVersionString { get; set; }
        public List<DefinitionDto> Definitions { get; set; } = new List<DefinitionDto>();
        public List<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
        public List<TranslationDto> Translations { get; set; } = new List<TranslationDto>();
    }
}
