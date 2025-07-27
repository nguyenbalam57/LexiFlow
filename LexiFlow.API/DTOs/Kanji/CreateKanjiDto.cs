using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for creating a new Kanji
    /// </summary>
    public class CreateKanjiDto
    {
        /// <summary>
        /// Kanji character
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Character { get; set; }

        /// <summary>
        /// On reading (Chinese-derived)
        /// </summary>
        [StringLength(100)]
        public string OnYomi { get; set; }

        /// <summary>
        /// Kun reading (Japanese-derived)
        /// </summary>
        [StringLength(100)]
        public string KunYomi { get; set; }

        /// <summary>
        /// Number of strokes
        /// </summary>
        public int StrokeCount { get; set; }

        /// <summary>
        /// JLPT level (N5-N1)
        /// </summary>
        [StringLength(20)]
        public string JLPTLevel { get; set; }

        /// <summary>
        /// School grade level
        /// </summary>
        [StringLength(20)]
        public string Grade { get; set; }

        /// <summary>
        /// Radical name
        /// </summary>
        [StringLength(50)]
        public string RadicalName { get; set; }

        /// <summary>
        /// Stroke order
        /// </summary>
        public string StrokeOrder { get; set; }

        /// <summary>
        /// Kanji meanings in different languages
        /// </summary>
        public List<CreateKanjiMeaningDto> Meanings { get; set; } = new List<CreateKanjiMeaningDto>();

        /// <summary>
        /// Examples of Kanji usage
        /// </summary>
        public List<CreateKanjiExampleDto> Examples { get; set; } = new List<CreateKanjiExampleDto>();

        /// <summary>
        /// Components of the Kanji
        /// </summary>
        public List<AddKanjiComponentDto> Components { get; set; } = new List<AddKanjiComponentDto>();
    }
}
