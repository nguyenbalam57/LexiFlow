using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for Kanji details
    /// </summary>
    public class KanjiDto
    {
        /// <summary>
        /// Kanji ID
        /// </summary>
        public int KanjiID { get; set; }

        /// <summary>
        /// Kanji character
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// On reading (Chinese-derived)
        /// </summary>
        public string OnYomi { get; set; }

        /// <summary>
        /// Kun reading (Japanese-derived)
        /// </summary>
        public string KunYomi { get; set; }

        /// <summary>
        /// Number of strokes
        /// </summary>
        public int StrokeCount { get; set; }

        /// <summary>
        /// JLPT level (N5-N1)
        /// </summary>
        public string JLPTLevel { get; set; }

        /// <summary>
        /// School grade level
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Radical name
        /// </summary>
        public string RadicalName { get; set; }

        /// <summary>
        /// Stroke order
        /// </summary>
        public string StrokeOrder { get; set; }

        /// <summary>
        /// Status (Active, Inactive, etc.)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ID of the user who created this Kanji
        /// </summary>
        public int CreatedByUserID { get; set; }

        /// <summary>
        /// ID of the user who last modified this Kanji
        /// </summary>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Row version for concurrency control
        /// </summary>
        public string RowVersionString { get; set; }

        /// <summary>
        /// Kanji meanings in different languages
        /// </summary>
        public List<KanjiMeaningDto> Meanings { get; set; } = new List<KanjiMeaningDto>();

        /// <summary>
        /// Examples of Kanji usage
        /// </summary>
        public List<KanjiExampleDto> Examples { get; set; } = new List<KanjiExampleDto>();

        /// <summary>
        /// Components of the Kanji
        /// </summary>
        public List<KanjiComponentMappingDto> Components { get; set; } = new List<KanjiComponentMappingDto>();
    }

}
