using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho tạo mục tiêu học tập
    /// </summary>
    public class CreateStudyGoalDto
    {
        /// <summary>
        /// Loại mục tiêu
        /// </summary>
        [Required]
        [StringLength(50)]
        public string GoalType { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Giá trị mục tiêu
        /// </summary>
        [Required]
        public int TargetValue { get; set; }

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        public List<CreateStudyTaskDto> Tasks { get; set; } = new List<CreateStudyTaskDto>();
    }
}
