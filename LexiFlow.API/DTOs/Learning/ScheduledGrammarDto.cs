namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho ngữ pháp trong lịch học
    /// </summary>
    public class ScheduledGrammarDto
    {
        /// <summary>
        /// ID ngữ pháp
        /// </summary>
        public int GrammarID { get; set; }

        /// <summary>
        /// Tên điểm ngữ pháp
        /// </summary>
        public string GrammarPoint { get; set; }

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
