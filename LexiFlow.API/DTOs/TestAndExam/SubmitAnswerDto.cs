using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho nộp câu trả lời
    /// </summary>
    public class SubmitAnswerDto
    {
        /// <summary>
        /// ID câu hỏi
        /// </summary>
        [Required]
        public int QuestionID { get; set; }

        /// <summary>
        /// Câu trả lời của người dùng
        /// </summary>
        public string UserAnswer { get; set; }

        /// <summary>
        /// ID lựa chọn đã chọn
        /// </summary>
        public int? SelectedOptionID { get; set; }

        /// <summary>
        /// Thời gian trả lời (giây)
        /// </summary>
        public int? ResponseTimeSeconds { get; set; }
    }
}
