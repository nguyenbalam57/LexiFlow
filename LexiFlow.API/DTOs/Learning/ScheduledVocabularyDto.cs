namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho từ vựng trong lịch học
    /// </summary>
    public class ScheduledVocabularyDto
    {
        /// <summary>
        /// ID từ vựng
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Từ tiếng Nhật
        /// </summary>
        public string Japanese { get; set; }

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
