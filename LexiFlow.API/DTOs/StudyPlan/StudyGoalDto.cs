using System;
using System.Collections.Generic;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho mục tiêu học tập
    /// </summary>
    public class StudyGoalDto
    {
        /// <summary>
        /// ID mục tiêu
        /// </summary>
        public int GoalId { get; set; }

        /// <summary>
        /// ID mục tiêu (alias for compatibility)
        /// </summary>
        public int GoalID { get => GoalId; set => GoalId = value; }

        /// <summary>
        /// ID kế hoạch
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// ID kế hoạch (alias for compatibility)
        /// </summary>
        public int PlanID { get => PlanId; set => PlanId = value; }

        /// <summary>
        /// Tên mục tiêu
        /// </summary>
        public string GoalName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID cấp độ
        /// </summary>
        public int? LevelId { get; set; }

        /// <summary>
        /// ID chủ đề
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// Ngày mục tiêu
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Ngày đến hạn (alias for compatibility)
        /// </summary>
        public DateTime? DueDate { get => TargetDate; set => TargetDate = value; }

        /// <summary>
        /// Mức độ quan trọng (1-5)
        /// </summary>
        public int? Importance { get; set; }

        /// <summary>
        /// Mức độ khó (1-5)
        /// </summary>
        public int? Difficulty { get; set; }

        /// <summary>
        /// Loại mục tiêu
        /// </summary>
        public string GoalType { get; set; } = string.Empty;

        /// <summary>
        /// Phạm vi mục tiêu
        /// </summary>
        public string GoalScope { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng mục tiêu
        /// </summary>
        public int? TargetCount { get; set; }

        /// <summary>
        /// Giá trị mục tiêu (alias for compatibility)
        /// </summary>
        public int TargetValue { get => TargetCount ?? 0; set => TargetCount = value; }

        /// <summary>
        /// Giá trị hiện tại
        /// </summary>
        public int CurrentValue { get; set; } = 0;

        /// <summary>
        /// Đơn vị đo lường
        /// </summary>
        public string MeasurementUnit { get; set; } = string.Empty;

        /// <summary>
        /// Tiêu chí thành công
        /// </summary>
        public string SuccessCriteria { get; set; } = string.Empty;

        /// <summary>
        /// Phương pháp xác minh
        /// </summary>
        public string VerificationMethod { get; set; } = string.Empty;

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Ngày hoàn thành
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Phần trăm hoàn thành
        /// </summary>
        public float ProgressPercentage { get; set; } = 0;

        /// <summary>
        /// Phần trăm hoàn thành (alias for compatibility)
        /// </summary>
        public float CompletionPercentage { get => ProgressPercentage; set => ProgressPercentage = value; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; } = "NotStarted";

        /// <summary>
        /// Số giờ ước tính
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Nguồn lực cần thiết
        /// </summary>
        public string RequiredResources { get; set; } = string.Empty;

        /// <summary>
        /// ID mục tiêu cha
        /// </summary>
        public int? ParentGoalId { get; set; }

        /// <summary>
        /// Phụ thuộc vào
        /// </summary>
        public string DependsOn { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        public List<StudyTaskDto> Tasks { get; set; } = new List<StudyTaskDto>();

        /// <summary>
        /// Danh sách mục tiêu con
        /// </summary>
        public List<StudyGoalDto> ChildGoals { get; set; } = new List<StudyGoalDto>();
    }
}
