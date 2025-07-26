using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class DefinitionDto
    {
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;

        [StringLength(50)]
        public string PartOfSpeech { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;
    }
}
