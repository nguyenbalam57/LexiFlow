namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho thống kê học tập hàng ngày
    /// </summary>
    public class DailyStudyStatsDto
    {
        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Số phút học
        /// </summary>
        public int StudyTimeMinutes { get; set; }

        /// <summary>
        /// Số từ vựng học mới
        /// </summary>
        public int NewVocabulary { get; set; }

        /// <summary>
        /// Số từ vựng ôn tập
        /// </summary>
        public int ReviewedVocabulary { get; set; }

        /// <summary>
        /// Số kanji học mới
        /// </summary>
        public int NewKanji { get; set; }

        /// <summary>
        /// Số kanji ôn tập
        /// </summary>
        public int ReviewedKanji { get; set; }

        /// <summary>
        /// Số điểm ngữ pháp học mới
        /// </summary>
        public int NewGrammar { get; set; }

        /// <summary>
        /// Số điểm ngữ pháp ôn tập
        /// </summary>
        public int ReviewedGrammar { get; set; }
    }
}
