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
        /// ID m?c ti�u
        /// </summary>
        public int GoalId { get; set; }

        /// <summary>
        /// T�n nhi?m v?
        /// </summary>
        public string TaskName { get; set; } = string.Empty;

        /// <summary>
        /// M� t? chi ti?t
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Th?i gian ??c t�nh
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
        /// ?? ?u ti�n (1-5)
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
        /// Ng�y d? ki?n
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Ng�y ??n h?n
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// C� r�ng bu?c th?i gian
        /// </summary>
        public bool HasTimeConstraint { get; set; } = false;

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string RequiredResources { get; set; } = string.Empty;

        /// <summary>
        /// URL t�i li?u ?�nh k�m
        /// </summary>
        public string AttachmentUrls { get; set; } = string.Empty;

        /// <summary>
        /// Nhi?m v? b?t bu?c
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// ?� ho�n th�nh
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Th?i ?i?m ho�n th�nh
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Tr?ng th�i
        /// </summary>
        public string Status { get; set; } = "NotStarted";

        /// <summary>
        /// Ph?n tr?m ho�n th�nh
        /// </summary>
        public float CompletionPercentage { get; set; } = 0f;

        /// <summary>
        /// L� nhi?m v? l?p l?i
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
        /// C�i ??t nh?c nh?
        /// </summary>
        public string ReminderSettings { get; set; } = string.Empty;

        /// <summary>
        /// Ph? thu?c v�o c�c task kh�c
        /// </summary>
        public string Dependencies { get; set; } = string.Empty;

        /// <summary>
        /// ?i?u ki?n ho�n th�nh
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
        /// Tr?ng th�i ho?t ??ng
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Th�ng tin m?c ti�u
        /// </summary>
        public StudyGoalDto? Goal { get; set; }

        /// <summary>
        /// Danh s�ch ho�n th�nh
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
        /// Th�ng tin ng??i t?o
        /// </summary>
        public string CreatedByName { get; set; } = string.Empty;

        /// <summary>
        /// Th�ng tin ng??i c?p nh?t
        /// </summary>
        public string ModifiedByName { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO chi ti?t cho th�ng tin ho�n th�nh nhi?m v?
    /// </summary>
    public class DetailedTaskCompletionDto
    {
        /// <summary>
        /// ID ho�n th�nh
        /// </summary>
        public int CompletionId { get; set; }

        /// <summary>
        /// ID nhi?m v?
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Ng�y ho�n th�nh
        /// </summary>
        public DateTime CompletionDate { get; set; }

        /// <summary>
        /// Tr?ng th�i ho�n th�nh (0-100)
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
        /// Ng??i ho�n th�nh
        /// </summary>
        public int? CompletedByUserId { get; set; }

        /// <summary>
        /// T�n ng??i ho�n th�nh
        /// </summary>
        public string CompletedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// ?? kh� (1-5)
        /// </summary>
        public int? Difficulty { get; set; }

        /// <summary>
        /// ?? h�i l�ng (1-5)
        /// </summary>
        public int? Satisfaction { get; set; }

        /// <summary>
        /// Hi?u qu? (1-5)
        /// </summary>
        public int? Effectiveness { get; set; }

        /// <summary>
        /// Ghi ch�
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
        /// S? c�u ?�ng
        /// </summary>
        public int? CorrectCount { get; set; }

        /// <summary>
        /// T?ng s? c�u
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// D? li?u ho�n th�nh (JSON)
        /// </summary>
        public string CompletionData { get; set; } = string.Empty;

        /// <summary>
        /// S? li?u h?c t?p (JSON)
        /// </summary>
        public string LearningMetrics { get; set; } = string.Empty;

        /// <summary>
        /// ID th?c th? li�n quan
        /// </summary>
        public int? RelatedEntityId { get; set; }

        /// <summary>
        /// Lo?i th?c th? li�n quan
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
        /// Tr?ng th�i ho?t ??ng
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}