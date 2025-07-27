using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for Kanji search request
    /// </summary>
    public class KanjiSearchRequestDto
    {
        /// <summary>
        /// Search query (can match character, readings, meanings)
        /// </summary>
        public string Query { get; set; } = "";

        /// <summary>
        /// JLPT level filter
        /// </summary>
        public string JLPTLevel { get; set; } = "";

        /// <summary>
        /// School grade filter
        /// </summary>
        public int? Grade { get; set; }

        /// <summary>
        /// Stroke count filter
        /// </summary>
        public int? StrokeCount { get; set; }

        /// <summary>
        /// Radical filter
        /// </summary>
        public string Radical { get; set; } = "";

        /// <summary>
        /// Maximum number of results to return
        /// </summary>
        [Range(1, 100)]
        public int MaxResults { get; set; } = 20;
    }
}
