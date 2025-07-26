namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho kanji trong lịch học
    /// </summary>
    public class ScheduledKanjiDto
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
        /// Loại học tập
        /// </summary>
        public string StudyType { get; set; } // New, Review

        /// <summary>
        /// Mức độ ưu tiên
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; }
    }

}
