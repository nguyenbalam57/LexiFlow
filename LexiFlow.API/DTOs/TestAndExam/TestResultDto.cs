namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho kết quả kiểm tra
    /// </summary>
    public class TestResultDto
    {
        /// <summary>
        /// ID kết quả
        /// </summary>
        public int ResultID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// ID bài kiểm tra
        /// </summary>
        public int ExamID { get; set; }

        /// <summary>
        /// Tên bài kiểm tra
        /// </summary>
        public string ExamName { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Điểm số
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// Phần trăm điểm
        /// </summary>
        public float ScorePercentage { get; set; }

        /// <summary>
        /// Đạt
        /// </summary>
        public bool IsPassed { get; set; }

        /// <summary>
        /// Danh sách câu trả lời
        /// </summary>
        public List<TestAnswerDto> Answers { get; set; } = new List<TestAnswerDto>();
    }
}
