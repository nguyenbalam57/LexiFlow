using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TestAndExam
{
    /// <summary>
    /// DTO cho tạo bài kiểm tra
    /// </summary>
    public class CreateExamDto
    {
        /// <summary>
        /// Tên bài kiểm tra
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        /// <summary>
        /// Loại bài kiểm tra
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ExamType { get; set; }

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        [StringLength(10)]
        public string JLPTLevel { get; set; }

        /// <summary>
        /// Thời gian làm bài (phút)
        /// </summary>
        [Required]
        [Range(1, 300)]
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Điểm đỗ
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int PassingScore { get; set; }

        /// <summary>
        /// Danh sách phần thi
        /// </summary>
        public List<CreateExamSectionDto> Sections { get; set; } = new List<CreateExamSectionDto>();
    }
}
