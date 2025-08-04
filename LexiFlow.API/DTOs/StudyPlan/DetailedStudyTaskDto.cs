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
        public string TaskName { get; set; }

        /// <summary>
        /// M� t? chi ti?t
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Th?i gian ??c t�nh
        /// </summary>
        public int? EstimatedDuration { get; set; }

        /// <summary>
        /// ??n v? th?i gian
        /// </summary>
        public string DurationUnit { get; set; }

        /// <summary>
        /// ID m?c trong k? ho?ch h?c t?p
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// ?? ?u ti�n (1-5)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Lo?i nhi?m v?
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// Danh m?c nhi?m v?
        /// </summary>
        public string TaskCategory { get; set; }

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
        public bool HasTimeConstraint { get; set; }

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string RequiredResources { get; set; }

        /// <summary>
        /// URL t�i li?u ?�nh k�m
        /// </summary>
        public string AttachmentUrls { get; set; }

        /// <summary>
        /// Nhi?m v? b?t bu?c
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// ?� ho�n th�nh
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Th?i ?i?m ho�n th�nh
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Tr?ng th�i
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Ph?n tr?m ho�n th�nh
        /// </summary>
        public float CompletionPercentage { get; set; }

        /// <summary>
        /// L� nhi?m v? l?p l?i
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// M?u l?p l?i
        /// </summary>
        public string RecurrencePattern { get; set; }

        /// <summary>
        /// B?t nh?c nh?
        /// </summary>
        public bool EnableReminders { get; set; }

        /// <summary>
        /// C�i ??t nh?c nh?
        /// </summary>
        public string ReminderSettings { get; set; }

        /// <summary>
        /// Ph? thu?c v�o c�c task kh�c
        /// </summary>
        public string Dependencies { get; set; }

        /// <summary>
        /// ?i?u ki?n ho�n th�nh
        /// </summary>
        public string CompletionConditions { get; set; }

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
        public bool IsActive { get; set; }

        /// <summary>
        /// Danh s�ch ho�n th�nh
        /// </summary>
        public List<TaskCompletionDto> TaskCompletions { get; set; } = new List<TaskCompletionDto>();
    }

    /// <summary>
    /// DTO cho th�ng tin ho�n th�nh nhi?m v?
    /// </summary>
    public class TaskCompletionDto
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
        public string DurationUnit { get; set; }

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
        public string Notes { get; set; }

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
    }
}