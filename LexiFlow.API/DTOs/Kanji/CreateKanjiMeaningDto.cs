using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for creating a Kanji meaning
    /// </summary>
    public class CreateKanjiMeaningDto
    {
        /// <summary>
        /// Meaning text
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Meaning { get; set; }

        /// <summary>
        /// Language code (e.g., vi, en)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Language { get; set; }

        /// <summary>
        /// Display order
        /// </summary>
        public int SortOrder { get; set; }
    }
}
