using System;
using System.Collections.Generic;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO ??y ?? chi ti?t cho nhi?m v? h?c t?p
    /// </summary>
    public class DetailedStudyTaskDto
    {
        /// <summary>
        /// ID nhi?m v?
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// ID m?c tiêu
        /// </summary>
        public int GoalId { get; set; }

        /// <summary>
        /// Tên nhi?m v?
        /// </summary>
        public string TaskName { get; set; } = string.Empty;

        /// <summary>
        /// Mô t? chi ti?t
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Th?i gian ??c tính
        /// </summary>
        public int? EstimatedDuration { get; set; }

        /// <summary>
        /// ??n v? th?i gian
        /// </summary>
        public string DurationUnit { get; set; } = "Minutes";

        /// <summary>
        /// ID m?c trong k? ho?ch h?c t?p
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// ?? ?u tiên (1-5)
        /// </summary>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Lo?i nhi?m v?
        /// </summary>
        public string TaskType { get; set; } = string.Empty;

        /// <summary>
        /// Danh m?c nhi?m v?
        /// </summary>
        public string TaskCategory { get; set; } = string.Empty;

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
        public bool HasTimeConstraint { get; set; } = false;

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string RequiredResources { get; set; } = string.Empty;

        /// <summary>
        /// URL tài li?u ?ính kèm
        /// </summary>
        public string AttachmentUrls { get; set; } = string.Empty;

        /// <summary>
        /// Nhi?m v? b?t bu?c
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// ?ã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Th?i ?i?m hoàn thành
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Tr?ng thái
        /// </summary>
        public string Status { get; set; } = "NotStarted";

        /// <summary>
        /// Ph?n tr?m hoàn thành
        /// </summary>
        public float CompletionPercentage { get; set; } = 0f;

        /// <summary>
        /// Là nhi?m v? l?p l?i
        /// </summary>
        public bool IsRecurring { get; set; } = false;

        /// <summary>
        /// M?u l?p l?i
        /// </summary>
        public string RecurrencePattern { get; set; } = string.Empty;

        /// <summary>
        /// B?t nh?c nh?
        /// </summary>
        public bool EnableReminders { get; set; } = true;

        /// <summary>
        /// Cài ??t nh?c nh?
        /// </summary>
        public string ReminderSettings { get; set; } = string.Empty;

        /// <summary>
        /// Ph? thu?c vào các task khác
        /// </summary>
        public string Dependencies { get; set; } = string.Empty;

        /// <summary>
        /// ?i?u ki?n hoàn thành
        /// </summary>
        public string CompletionConditions { get; set; } = string.Empty;

        /// <summary>
        /// Th?i gian t?o
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Th?i gian c?p nh?t
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Tr?ng thái ho?t ??ng
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Thông tin m?c tiêu
        /// </summary>
        public StudyGoalDto? Goal { get; set; }

        /// <summary>
        /// Danh sách hoàn thành
        /// </summary>
        public List<DetailedTaskCompletionDto> TaskCompletions { get; set; } = new List<DetailedTaskCompletionDto>();

        /// <summary>
        /// Ng??i t?o
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Ng??i c?p nh?t
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Thông tin ng??i t?o
        /// </summary>
        public string CreatedByName { get; set; } = string.Empty;

        /// <summary>
        /// Thông tin ng??i c?p nh?t
        /// </summary>
        public string ModifiedByName { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO chi ti?t cho thông tin hoàn thành nhi?m v?
    /// </summary>
    public class DetailedTaskCompletionDto
    {
        /// <summary>
        /// ID hoàn thành
        /// </summary>
        public int CompletionId { get; set; }

        /// <summary>
        /// ID nhi?m v?
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Ngày hoàn thành
        /// </summary>
        public DateTime CompletionDate { get; set; }

        /// <summary>
        /// Tr?ng thái hoàn thành (0-100)
        /// </summary>
        public int? CompletionStatus { get; set; }

        /// <summary>
        /// Th?i gian th?c t?
        /// </summary>
        public int? ActualDuration { get; set; }

        /// <summary>
        /// ??n v? th?i gian
        /// </summary>
        public string DurationUnit { get; set; } = "Minutes";

        /// <summary>
        /// Ng??i hoàn thành
        /// </summary>
        public int? CompletedByUserId { get; set; }

        /// <summary>
        /// Tên ng??i hoàn thành
        /// </summary>
        public string CompletedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// ?? khó (1-5)
        /// </summary>
        public int? Difficulty { get; set; }

        /// <summary>
        /// ?? hài lòng (1-5)
        /// </summary>
        public int? Satisfaction { get; set; }

        /// <summary>
        /// Hi?u qu? (1-5)
        /// </summary>
        public int? Effectiveness { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Ph?n h?i
        /// </summary>
        public string Feedback { get; set; } = string.Empty;

        /// <summary>
        /// K?t qu? h?c t?p
        /// </summary>
        public string Outcome { get; set; } = string.Empty;

        /// <summary>
        /// ?i?m s?
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// S? câu ?úng
        /// </summary>
        public int? CorrectCount { get; set; }

        /// <summary>
        /// T?ng s? câu
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// D? li?u hoàn thành (JSON)
        /// </summary>
        public string CompletionData { get; set; } = string.Empty;

        /// <summary>
        /// S? li?u h?c t?p (JSON)
        /// </summary>
        public string LearningMetrics { get; set; } = string.Empty;

        /// <summary>
        /// ID th?c th? liên quan
        /// </summary>
        public int? RelatedEntityId { get; set; }

        /// <summary>
        /// Lo?i th?c th? liên quan
        /// </summary>
        public string RelatedEntityType { get; set; } = string.Empty;

        /// <summary>
        /// Th?i gian t?o
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Th?i gian c?p nh?t
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Tr?ng thái ho?t ??ng
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}