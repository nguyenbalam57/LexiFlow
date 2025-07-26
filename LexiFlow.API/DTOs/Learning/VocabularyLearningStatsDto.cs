namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho thống kê học từ vựng
    /// </summary>
    public class VocabularyLearningStatsDto
    {
        /// <summary>
        /// Tổng số từ vựng đã học
        /// </summary>
        public int TotalVocabularyLearned { get; set; }

        /// <summary>
        /// Số từ vựng đã thành thạo
        /// </summary>
        public int MasteredVocabulary { get; set; }

        /// <summary>
        /// Số từ vựng cần ôn tập
        /// </summary>
        public int VocabularyNeedingReview { get; set; }

        /// <summary>
        /// Tỷ lệ trả lời đúng
        /// </summary>
        public double CorrectAnswerRate { get; set; }

        /// <summary>
        /// Thống kê theo level
        /// </summary>
        public Dictionary<string, int> LearnedByLevel { get; set; } = new Dictionary<string, int>();
    }
}
