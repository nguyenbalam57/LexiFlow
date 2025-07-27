using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for creating a Kanji example meaning
    /// </summary>
    public class CreateKanjiExampleMeaningDto
    {
        /// <summary>
        /// The meaning text
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Meaning { get; set; }

        /// <summary>
        /// Language code (e.g., vi, en)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Language { get; set; } = "vi";  // Default to Vietnamese

        /// <summary>
        /// Order for display
        /// </summary>
        public int SortOrder { get; set; }
    }
}
