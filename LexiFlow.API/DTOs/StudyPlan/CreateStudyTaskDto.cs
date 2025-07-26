using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho tạo nhiệm vụ học tập
    /// </summary>
    public class CreateStudyTaskDto
    {
        /// <summary>
        /// Mô tả nhiệm vụ
        /// </summary>
        [Required]
        public string TaskDescription { get; set; }

        /// <summary>
        /// Độ ưu tiên
        /// </summary>
        public int Priority { get; set; } = 2;

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
