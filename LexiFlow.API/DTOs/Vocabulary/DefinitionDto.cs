using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class DefinitionDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;

        [StringLength(50)]
        public string PartOfSpeech { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;
    }
}
