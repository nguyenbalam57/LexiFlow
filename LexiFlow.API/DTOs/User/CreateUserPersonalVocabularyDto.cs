using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class CreateUserPersonalVocabularyDto
    {
        [Required]
        [StringLength(100)]
        public string Term { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }

        public string PersonalNote { get; set; }

        public string Context { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        public int Importance { get; set; } = 3;

        [StringLength(255)]
        public string Tags { get; set; }

        public bool IsPublic { get; set; } = false;

        public List<UserPersonalDefinitionDto> Definitions { get; set; } = new List<UserPersonalDefinitionDto>();

        public List<UserPersonalExampleDto> Examples { get; set; } = new List<UserPersonalExampleDto>();

        public List<UserPersonalTranslationDto> Translations { get; set; } = new List<UserPersonalTranslationDto>();
    }
}
