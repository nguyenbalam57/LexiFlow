using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho tạo lựa chọn câu hỏi
    /// </summary>
    public class CreateQuestionOptionDto
    {
        /// <summary>
        /// Nội dung lựa chọn
        /// </summary>
        [Required]
        public string OptionText { get; set; }

        /// <summary>
        /// Là đáp án đúng
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
