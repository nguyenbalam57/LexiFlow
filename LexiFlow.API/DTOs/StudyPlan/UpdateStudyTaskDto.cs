using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho c?p nh?t nhi?m v? h?c t?p
    /// </summary>
    public class UpdateStudyTaskDto
    {
        /// <summary>
        /// Tên nhi?m v?
        /// </summary>
        [StringLength(100)]
        public string? TaskName { get; set; }

        /// <summary>
        /// Mô t? nhi?m v?
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Th?i gian ??c tính
        /// </summary>
        public int? EstimatedDuration { get; set; }

        /// <summary>
        /// ??n v? th?i gian
        /// </summary>
        [StringLength(20)]
        public string? DurationUnit { get; set; }

        /// <summary>
        /// ?? ?u tiên (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Priority { get; set; }

        /// <summary>
        /// Lo?i nhi?m v?
        /// </summary>
        [StringLength(50)]
        public string? TaskType { get; set; }

        /// <summary>
        /// Danh m?c nhi?m v?
        /// </summary>
        [StringLength(50)]
        public string? TaskCategory { get; set; }

        /// <summary>
        /// Ngày d? ki?n
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Ngày ??n h?n
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Có ràng bu?c th?i gian
        /// </summary>
        public bool? HasTimeConstraint { get; set; }

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string? RequiredResources { get; set; }

        /// <summary>
        /// URL tài li?u ?ính kèm
        /// </summary>
        public string? AttachmentUrls { get; set; }

        /// <summary>
        /// Nhi?m v? b?t bu?c
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Tr?ng thái
        /// </summary>
        [StringLength(50)]
        public string? Status { get; set; }

        /// <summary>
        /// Ph?n tr?m hoàn thành
        /// </summary>
        [Range(0, 100)]
        public float? CompletionPercentage { get; set; }

        /// <summary>
        /// Là nhi?m v? l?p l?i
        /// </summary>
        public bool? IsRecurring { get; set; }

        /// <summary>
        /// M?u l?p l?i
        /// </summary>
        [StringLength(50)]
        public string? RecurrencePattern { get; set; }

        /// <summary>
        /// B?t nh?c nh?
        /// </summary>
        public bool? EnableReminders { get; set; }

        /// <summary>
        /// Cài ??t nh?c nh?
        /// </summary>
        [StringLength(255)]
        public string? ReminderSettings { get; set; }

        /// <summary>
        /// Ph? thu?c vào các task khác
        /// </summary>
        public string? Dependencies { get; set; }

        /// <summary>
        /// ?i?u ki?n hoàn thành
        /// </summary>
        public string? CompletionConditions { get; set; }

        /// <summary>
        /// Tr?ng thái ho?t ??ng
        /// </summary>
        public bool? IsActive { get; set; }
    }
}