namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO cho thống kê kanji
    /// </summary>
    public class KanjiStatisticsDto
    {
        /// <summary>
        /// Tổng số kanji
        /// </summary>
        public int TotalKanji { get; set; }

        /// <summary>
        /// Thống kê kanji theo JLPT level
        /// </summary>
        public Dictionary<string, int> KanjiByJLPTLevel { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Thống kê kanji theo grade
        /// </summary>
        public Dictionary<int, int> KanjiByGrade { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// Thống kê kanji theo số nét
        /// </summary>
        public Dictionary<int, int> KanjiByStrokeCount { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// Kanji được học nhiều nhất
        /// </summary>
        public List<MostStudiedKanjiDto> MostStudiedKanji { get; set; } = new List<MostStudiedKanjiDto>();
    }
}
