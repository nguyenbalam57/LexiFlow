namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO cho kanji được học nhiều nhất
    /// </summary>
    public class MostStudiedKanjiDto
    {
        /// <summary>
        /// ID kanji
        /// </summary>
        public int KanjiID { get; set; }

        /// <summary>
        /// Ký tự kanji
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        public string JLPTLevel { get; set; }

        /// <summary>
        /// Số lượt học
        /// </summary>
        public int StudyCount { get; set; }
    }
}
