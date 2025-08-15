using System;
using System.Collections.Generic;
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
        public string PlanName { get; set; } = string.Empty;

        /// <summary>
        /// Cấp độ mục tiêu
        /// </summary>
        [StringLength(10)]
        public string TargetLevel { get; set; } = string.Empty;

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
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Số phút mỗi ngày
        /// </summary>
        public int? MinutesPerDay { get; set; }

        /// <summary>
        /// Loại kế hoạch
        /// </summary>
        [StringLength(50)]
        public string PlanType { get; set; } = "General";

        /// <summary>
        /// Mức độ cường độ
        /// </summary>
        [StringLength(50)]
        public string Intensity { get; set; } = "Moderate";

        /// <summary>
        /// Số ngày học mỗi tuần
        /// </summary>
        public int? DaysPerWeek { get; set; }

        /// <summary>
        /// Bật nhắc nhở
        /// </summary>
        public bool EnableReminders { get; set; } = true;

        /// <summary>
        /// Cài đặt nhắc nhở
        /// </summary>
        public string ReminderSettings { get; set; } = string.Empty;

        /// <summary>
        /// Tự động điều chỉnh
        /// </summary>
        public bool AutoAdjust { get; set; } = true;

        /// <summary>
        /// Danh sách mục tiêu
        /// </summary>
        public List<CreateStudyGoalDto> Goals { get; set; } = new List<CreateStudyGoalDto>();
    }
}
