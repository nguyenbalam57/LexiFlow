namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho thống kê học kanji
    /// </summary>
    public class KanjiLearningStatsDto
    {
        /// <summary>
        /// Tổng số kanji đã học
        /// </summary>
        public int TotalKanjiLearned { get; set; }

        /// <summary>
        /// Số kanji đã thành thạo
        /// </summary>
        public int MasteredKanji { get; set; }

        /// <summary>
        /// Số kanji cần ôn tập
        /// </summary>
        public int KanjiNeedingReview { get; set; }

        /// <summary>
        /// Thống kê theo JLPT level
        /// </summary>
        public Dictionary<string, int> LearnedByJLPTLevel { get; set; } = new Dictionary<string, int>();
    }
}
