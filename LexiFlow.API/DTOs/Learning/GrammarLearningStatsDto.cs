namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho thống kê học ngữ pháp
    /// </summary>
    public class GrammarLearningStatsDto
    {
        /// <summary>
        /// Tổng số điểm ngữ pháp đã học
        /// </summary>
        public int TotalGrammarLearned { get; set; }

        /// <summary>
        /// Số điểm ngữ pháp đã thành thạo
        /// </summary>
        public int MasteredGrammar { get; set; }

        /// <summary>
        /// Số điểm ngữ pháp cần ôn tập
        /// </summary>
        public int GrammarNeedingReview { get; set; }

        /// <summary>
        /// Thống kê theo JLPT level
        /// </summary>
        public Dictionary<string, int> LearnedByJLPTLevel { get; set; } = new Dictionary<string, int>();
    }
}
