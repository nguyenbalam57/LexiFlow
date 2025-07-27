namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for Kanji example meaning
    /// </summary>
    public class KanjiExampleMeaningDto
    {
        /// <summary>
        /// ID of the meaning
        /// </summary>
        public int MeaningID { get; set; }

        /// <summary>
        /// The meaning text
        /// </summary>
        public string Meaning { get; set; }

        /// <summary>
        /// Language code (e.g., vi, en)
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Order for display
        /// </summary>
        public int SortOrder { get; set; }
    }
}
