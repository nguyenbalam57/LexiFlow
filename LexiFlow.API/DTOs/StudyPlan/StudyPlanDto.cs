namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO đầy đủ cho kế hoạch học tập
    /// </summary>
    public class StudyPlanDto
    {
        /// <summary>
        /// ID kế hoạch
        /// </summary>
        public int PlanID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Tên kế hoạch
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Cấp độ mục tiêu
        /// </summary>
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
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Trạng thái hiện tại
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// Phần trăm hoàn thành
        /// </summary>
        public float CompletionPercentage { get; set; }

        /// <summary>
        /// Lần cập nhật cuối
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Danh sách mục tiêu
        /// </summary>
        public List<StudyGoalDto> Goals { get; set; } = new List<StudyGoalDto>();
    }
}
