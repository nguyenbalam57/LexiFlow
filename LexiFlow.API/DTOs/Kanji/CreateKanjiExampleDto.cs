using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for creating a Kanji example
    /// </summary>
    public class CreateKanjiExampleDto
    {
        /// <summary>
        /// Japanese word or phrase
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }

        /// <summary>
        /// Kana reading
        /// </summary>
        [StringLength(100)]
        public string Kana { get; set; }

        /// <summary>
        /// Meanings in different languages
        /// </summary>
        public List<CreateKanjiExampleMeaningDto> Meanings { get; set; } = new List<CreateKanjiExampleMeaningDto>();
    }
}
