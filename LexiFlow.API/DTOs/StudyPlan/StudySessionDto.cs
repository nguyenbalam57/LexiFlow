using System;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho phiên h?c t?p
    /// </summary>
    public class StudySessionDto
    {
        /// <summary>
        /// ID phiên h?c t?p
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        /// ID ng??i dùng
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Th?i gian b?t ??u
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Th?i gian k?t thúc
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Th?i l??ng (giây)
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Lo?i phiên h?c t?p
        /// </summary>
        public string SessionType { get; set; } = string.Empty;

        /// <summary>
        /// Lo?i n?i dung
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// ID k? ho?ch h?c t?p
        /// </summary>
        public int? StudyPlanId { get; set; }

        /// <summary>
        /// S? m?c ?ã h?c
        /// </summary>
        public int ItemsStudied { get; set; }

        /// <summary>
        /// S? câu tr? l?i ?úng
        /// </summary>
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// S? câu tr? l?i sai
        /// </summary>
        public int WrongAnswers { get; set; }

        /// <summary>
        /// ?i?m s?
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// N?n t?ng
        /// </summary>
        public string Platform { get; set; } = string.Empty;

        /// <summary>
        /// ?ã ??ng b?
        /// </summary>
        public bool IsSynced { get; set; }

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