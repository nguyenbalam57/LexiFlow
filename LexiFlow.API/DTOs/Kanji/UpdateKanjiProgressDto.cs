using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for updating Kanji progress
    /// </summary>
    public class UpdateKanjiProgressDto
    {
        /// <summary>
        /// ID of the Kanji
        /// </summary>
        [Required]
        public int KanjiID { get; set; }

        /// <summary>
        /// Recognition level (0-10)
        /// </summary>
        [Range(0, 10)]
        public int RecognitionLevel { get; set; }

        /// <summary>
        /// Writing level (0-10)
        /// </summary>
        [Range(0, 10)]
        public int WritingLevel { get; set; }

        /// <summary>
        /// Whether the answer was correct in this practice session
        /// </summary>
        public bool CorrectAnswer { get; set; }

        /// <summary>
        /// User's personal notes
        /// </summary>
        public string Notes { get; set; }
    }
}
