using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarDefinitionDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        public string Usage { get; set; }

        public int SortOrder { get; set; }
    }
}
