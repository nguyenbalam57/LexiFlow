using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho tạo kế hoạch học tập
    /// </summary>
    public class CreateStudyPlanDto
    {
        /// <summary>
        /// Tên kế hoạch
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        /// <summary>
        /// Cấp độ mục tiêu
        /// </summary>
        [StringLength(10)]
        public string TargetLevel { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày mục tiêu
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Số phút mỗi ngày
        /// </summary>
        public int? MinutesPerDay { get; set; }

        /// <summary>
        /// Danh sách mục tiêu
        /// </summary>
        public List<CreateStudyGoalDto> Goals { get; set; } = new List<CreateStudyGoalDto>();
    }
}
