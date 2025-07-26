namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho kết quả mục học tập
    /// </summary>
    public class StudyItemResultDto
    {
        /// <summary>
        /// ID mục
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// Loại mục
        /// </summary>
        public string ItemType { get; set; } // Vocabulary, Kanji, Grammar

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Trả lời đúng
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Thời gian trả lời (giây)
        /// </summary>
        public int ResponseTimeSeconds { get; set; }

        /// <summary>
        /// Mức độ khó trước đó
        /// </summary>
        public int PreviousDifficulty { get; set; }

        /// <summary>
        /// Mức độ khó mới
        /// </summary>
        public int NewDifficulty { get; set; }

        /// <summary>
        /// Ngày ôn tập tiếp theo
        /// </summary>
        public DateTime? NextReviewDate { get; set; }
    }
}
