using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class UpdateGrammarProgressDto
    {
        [Required]
        public int GrammarID { get; set; }
        public int UnderstandingLevel { get; set; }
        public int UsageLevel { get; set; }
        public float? TestScore { get; set; }
        public string PersonalNotes { get; set; }
    }
}
