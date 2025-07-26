namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho nhiệm vụ học tập
    /// </summary>
    public class StudyTaskDto
    {
        /// <summary>
        /// ID nhiệm vụ
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// ID mục tiêu
        /// </summary>
        public int GoalID { get; set; }

        /// <summary>
        /// Mô tả nhiệm vụ
        /// </summary>
        public string TaskDescription { get; set; }

        /// <summary>
        /// Độ ưu tiên
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Ngày hoàn thành
        /// </summary>
        public DateTime? CompletionDate { get; set; }
    }
}
