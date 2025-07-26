using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class CreateGrammarDto
    {
        [Required]
        [StringLength(100)]
        public string GrammarPoint { get; set; }

        [StringLength(10)]
        public string JLPTLevel { get; set; }

        [StringLength(255)]
        public string Pattern { get; set; }

        public string Meaning { get; set; }

        public string Usage { get; set; }

        public string Notes { get; set; }

        public string Conjugation { get; set; }

        [StringLength(255)]
        public string RelatedGrammar { get; set; }

        public int? CategoryID { get; set; }

        public List<CreateGrammarExampleDto> Examples { get; set; } = new List<CreateGrammarExampleDto>();
    }
}
