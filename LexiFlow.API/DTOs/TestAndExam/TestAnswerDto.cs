namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho câu trả lời kiểm tra
    /// </summary>
    public class TestAnswerDto
    {
        /// <summary>
        /// ID câu trả lời
        /// </summary>
        public int AnswerID { get; set; }

        /// <summary>
        /// ID kết quả
        /// </summary>
        public int ResultID { get; set; }

        /// <summary>
        /// ID câu hỏi
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        /// Nội dung câu hỏi
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Câu trả lời của người dùng
        /// </summary>
        public string UserAnswer { get; set; }

        /// <summary>
        /// Đáp án đúng
        /// </summary>
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// Đúng
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Điểm
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Phản hồi
        /// </summary>
        public string Feedback { get; set; }
    }
}
