namespace LexiFlow.API.DTOs.User
{
    public class UserPersonalVocabularyDto
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Term { get; set; }
        public string LanguageCode { get; set; }
        public string Reading { get; set; }
        public string PersonalNote { get; set; }
        public string Context { get; set; }
        public string Source { get; set; }
        public int Importance { get; set; }
        public string Tags { get; set; }
        public string ImagePath { get; set; }
        public string AudioPath { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RowVersionString { get; set; }
        public List<UserPersonalDefinitionDto> Definitions { get; set; } = new List<UserPersonalDefinitionDto>();
        public List<UserPersonalExampleDto> Examples { get; set; } = new List<UserPersonalExampleDto>();
        public List<UserPersonalTranslationDto> Translations { get; set; } = new List<UserPersonalTranslationDto>();
    }
}
