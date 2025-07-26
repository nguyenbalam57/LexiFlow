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
        public int GoalID { get; set; }

        /// <summary>
        /// ID kế hoạch
        /// </summary>
        public int PlanID { get; set; }

        /// <summary>
        /// Loại mục tiêu
        /// </summary>
        public string GoalType { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Giá trị mục tiêu
        /// </summary>
        public int TargetValue { get; set; }

        /// <summary>
        /// Giá trị hiện tại
        /// </summary>
        public int CurrentValue { get; set; }

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        public List<StudyTaskDto> Tasks { get; set; } = new List<StudyTaskDto>();
    }

}
