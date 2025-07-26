namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho kết quả phiên học tập
    /// </summary>
    public class StudySessionResultDto
    {
        /// <summary>
        /// ID phiên học
        /// </summary>
        public int SessionID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Loại phiên học
        /// </summary>
        public string SessionType { get; set; } // Vocabulary, Kanji, Grammar, Mixed

        /// <summary>
        /// Tổng số câu hỏi
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Số câu trả lời đúng
        /// </summary>
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// Điểm số
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Điểm kinh nghiệm nhận được
        /// </summary>
        public int ExperienceGained { get; set; }

        /// <summary>
        /// Danh sách kết quả chi tiết
        /// </summary>
        public List<StudyItemResultDto> ItemResults { get; set; } = new List<StudyItemResultDto>();

        /// <summary>
        /// Khuyến nghị học tập
        /// </summary>
        public List<string> Recommendations { get; set; } = new List<string>();
    }
}
