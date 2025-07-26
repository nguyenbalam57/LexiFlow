using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho tạo câu hỏi
    /// </summary>
    public class CreateQuestionDto
    {
        /// <summary>
        /// Nội dung câu hỏi
        /// </summary>
        [Required]
        public string QuestionText { get; set; }

        /// <summary>
        /// Loại câu hỏi
        /// </summary>
        [Required]
        [StringLength(50)]
        public string QuestionType { get; set; }

        /// <summary>
        /// Điểm số
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int Points { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Độ khó
        /// </summary>
        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        /// <summary>
        /// URL media
        /// </summary>
        public string MediaURL { get; set; }

        /// <summary>
        /// Danh sách lựa chọn
        /// </summary>
        public List<CreateQuestionOptionDto> Options { get; set; } = new List<CreateQuestionOptionDto>();
    }
}
