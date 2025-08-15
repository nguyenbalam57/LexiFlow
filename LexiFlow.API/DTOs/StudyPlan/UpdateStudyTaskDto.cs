using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho c?p nh?t nhi?m v? h?c t?p
    /// </summary>
    public class UpdateStudyTaskDto
    {
        /// <summary>
        /// T�n nhi?m v?
        /// </summary>
        [StringLength(100)]
        public string? TaskName { get; set; }

        /// <summary>
        /// M� t? nhi?m v?
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Th?i gian ??c t�nh
        /// </summary>
        public int? EstimatedDuration { get; set; }

        /// <summary>
        /// ??n v? th?i gian
        /// </summary>
        [StringLength(20)]
        public string? DurationUnit { get; set; }

        /// <summary>
        /// ?? ?u ti�n (1-5)
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
        public bool? HasTimeConstraint { get; set; }

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string? RequiredResources { get; set; }

        /// <summary>
        /// URL t�i li?u ?�nh k�m
        /// </summary>
        public string? AttachmentUrls { get; set; }

        /// <summary>
        /// Nhi?m v? b?t bu?c
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Tr?ng th�i
        /// </summary>
        [StringLength(50)]
        public string? Status { get; set; }

        /// <summary>
        /// Ph?n tr?m ho�n th�nh
        /// </summary>
        [Range(0, 100)]
        public float? CompletionPercentage { get; set; }

        /// <summary>
        /// L� nhi?m v? l?p l?i
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
        /// C�i ??t nh?c nh?
        /// </summary>
        [StringLength(255)]
        public string? ReminderSettings { get; set; }

        /// <summary>
        /// Ph? thu?c v�o c�c task kh�c
        /// </summary>
        public string? Dependencies { get; set; }

        /// <summary>
        /// ?i?u ki?n ho�n th�nh
        /// </summary>
        public string? CompletionConditions { get; set; }

        /// <summary>
        /// Tr?ng th�i ho?t ??ng
        /// </summary>
        public bool? IsActive { get; set; }
    }
}