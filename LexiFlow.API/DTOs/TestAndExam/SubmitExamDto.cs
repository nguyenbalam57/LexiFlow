using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho nộp bài kiểm tra
    /// </summary>
    public class SubmitExamDto
    {
        /// <summary>
        /// ID bài kiểm tra
        /// </summary>
        [Required]
        public int ExamID { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Danh sách câu trả lời
        /// </summary>
        public List<SubmitAnswerDto> Answers { get; set; } = new List<SubmitAnswerDto>();
    }
}
