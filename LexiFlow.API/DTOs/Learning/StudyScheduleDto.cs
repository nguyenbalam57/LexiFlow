namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho lịch học tập
    /// </summary>
    public class StudyScheduleDto
    {
        /// <summary>
        /// ID lịch học
        /// </summary>
        public int ScheduleID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Danh sách từ vựng cần học
        /// </summary>
        public List<ScheduledVocabularyDto> VocabularyToLearn { get; set; } = new List<ScheduledVocabularyDto>();

        /// <summary>
        /// Danh sách kanji cần học
        /// </summary>
        public List<ScheduledKanjiDto> KanjiToLearn { get; set; } = new List<ScheduledKanjiDto>();

        /// <summary>
        /// Danh sách ngữ pháp cần học
        /// </summary>
        public List<ScheduledGrammarDto> GrammarToLearn { get; set; } = new List<ScheduledGrammarDto>();

        /// <summary>
        /// Tổng thời gian dự kiến (phút)
        /// </summary>
        public int EstimatedTimeMinutes { get; set; }

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
