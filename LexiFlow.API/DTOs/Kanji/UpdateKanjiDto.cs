using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for updating an existing Kanji
    /// </summary>
    public class UpdateKanjiDto
    {
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
        /// Status (Active, Inactive, etc.)
        /// </summary>
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Kanji meanings in different languages
        /// </summary>
        public List<UpdateKanjiMeaningDto> Meanings { get; set; } = new List<UpdateKanjiMeaningDto>();

        /// <summary>
        /// Examples of Kanji usage
        /// </summary>
        //public List<UpdateKanjiExampleDto> Examples { get; set; } = new List<UpdateKanjiExampleDto>();

        /// <summary>
        /// Components of the Kanji
        /// </summary>
        //public List<UpdateKanjiComponentMappingDto> Components { get; set; } = new List<UpdateKanjiComponentMappingDto>();

        /// <summary>
        /// Row version for concurrency control
        /// </summary>
        [Required]
        public string RowVersionString { get; set; }
    }
}
