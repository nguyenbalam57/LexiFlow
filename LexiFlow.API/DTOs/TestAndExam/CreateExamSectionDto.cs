using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho tạo phần thi
    /// </summary>
    public class CreateExamSectionDto
    {
        /// <summary>
        /// Tên phần thi
        /// </summary>
        [Required]
        [StringLength(100)]
        public string SectionName { get; set; }

        /// <summary>
        /// Loại phần thi
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SectionType { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Thời gian làm bài (phút)
        /// </summary>
        [Required]
        [Range(1, 180)]
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Hướng dẫn
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Danh sách câu hỏi
        /// </summary>
        public List<CreateQuestionDto> Questions { get; set; } = new List<CreateQuestionDto>();
    }
}
