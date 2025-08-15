using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho tạo nhiệm vụ học tập
    /// </summary>
    public class CreateStudyTaskDto
    {
        /// <summary>
        /// Tên nhiệm vụ
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TaskName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả nhiệm vụ
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian ước tính
        /// </summary>
        public int? EstimatedDuration { get; set; }

        /// <summary>
        /// Đơn vị thời gian
        /// </summary>
        [StringLength(20)]
        public string DurationUnit { get; set; } = "Minutes";

        /// <summary>
        /// ID mục trong kế hoạch học tập
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// Độ ưu tiên (1-5)
        /// </summary>
        [Range(1, 5)]
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Loại nhiệm vụ
        /// </summary>
        [StringLength(50)]
        public string TaskType { get; set; } = "Study";

        /// <summary>
        /// Danh mục nhiệm vụ
        /// </summary>
        [StringLength(50)]
        public string TaskCategory { get; set; } = string.Empty;

        /// <summary>
        /// Ngày dự kiến
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Có ràng buộc thời gian
        /// </summary>
        public bool HasTimeConstraint { get; set; } = false;

        /// <summary>
        /// Nguồn lực cần thiết
        /// </summary>
        public string RequiredResources { get; set; } = string.Empty;

        /// <summary>
        /// URL tài liệu đính kèm
        /// </summary>
        public string AttachmentUrls { get; set; } = string.Empty;

        /// <summary>
        /// Nhiệm vụ bắt buộc
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// Là nhiệm vụ lặp lại
        /// </summary>
        public bool IsRecurring { get; set; } = false;

        /// <summary>
        /// Mẫu lặp lại
        /// </summary>
        [StringLength(50)]
        public string RecurrencePattern { get; set; } = string.Empty;

        /// <summary>
        /// Bật nhắc nhở
        /// </summary>
        public bool EnableReminders { get; set; } = true;

        /// <summary>
        /// Cài đặt nhắc nhở
        /// </summary>
        [StringLength(255)]
        public string ReminderSettings { get; set; } = string.Empty;

        /// <summary>
        /// Phụ thuộc vào các task khác
        /// </summary>
        public string Dependencies { get; set; } = string.Empty;

        /// <summary>
        /// Điều kiện hoàn thành
        /// </summary>
        public string CompletionConditions { get; set; } = string.Empty;
    }
}
