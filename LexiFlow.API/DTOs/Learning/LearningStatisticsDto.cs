namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho thống kê học tập tổng hợp
    /// </summary>
    public class LearningStatisticsDto
    {
        /// <summary>
        /// Thống kê từ vựng
        /// </summary>
        public VocabularyLearningStatsDto VocabularyStats { get; set; } = new VocabularyLearningStatsDto();

        /// <summary>
        /// Thống kê kanji
        /// </summary>
        public KanjiLearningStatsDto KanjiStats { get; set; } = new KanjiLearningStatsDto();

        /// <summary>
        /// Thống kê ngữ pháp
        /// </summary>
        public GrammarLearningStatsDto GrammarStats { get; set; } = new GrammarLearningStatsDto();

        /// <summary>
        /// Thống kê theo thời gian
        /// </summary>
        public List<DailyStudyStatsDto> DailyStats { get; set; } = new List<DailyStudyStatsDto>();

        /// <summary>
        /// Thời gian học tổng cộng (phút)
        /// </summary>
        public int TotalStudyTimeMinutes { get; set; }

        /// <summary>
        /// Thành tích học tập
        /// </summary>
        public List<AchievementDto> Achievements { get; set; } = new List<AchievementDto>();
    }

}
