using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class UpdateKanjiProgressDto
    {
        [Required]
        public int KanjiID { get; set; }
        public int RecognitionLevel { get; set; }
        public int WritingLevel { get; set; }
        public bool CorrectAnswer { get; set; }
        public string Notes { get; set; }
    }
}
