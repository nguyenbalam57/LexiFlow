using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho c?p nh?t m?c tiêu h?c t?p
    /// </summary>
    public class UpdateStudyGoalDto
    {
        /// <summary>
        /// Tên m?c tiêu
        /// </summary>
        [StringLength(100)]
        public string? GoalName { get; set; }

        /// <summary>
        /// Mô t?
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID c?p ??
        /// </summary>
        public int? LevelId { get; set; }

        /// <summary>
        /// ID ch? ??
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// Ngày m?c tiêu
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// M?c ?? quan tr?ng (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Importance { get; set; }

        /// <summary>
        /// M?c ?? khó (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; }

        /// <summary>
        /// Lo?i m?c tiêu
        /// </summary>
        [StringLength(50)]
        public string? GoalType { get; set; }

        /// <summary>
        /// Ph?m vi m?c tiêu
        /// </summary>
        [StringLength(50)]
        public string? GoalScope { get; set; }

        /// <summary>
        /// S? l??ng m?c tiêu
        /// </summary>
        public int? TargetCount { get; set; }

        /// <summary>
        /// ??n v? ?o l??ng
        /// </summary>
        [StringLength(50)]
        public string? MeasurementUnit { get; set; }

        /// <summary>
        /// Tiêu chí thành công
        /// </summary>
        public string? SuccessCriteria { get; set; }

        /// <summary>
        /// Ph??ng pháp xác minh
        /// </summary>
        public string? VerificationMethod { get; set; }

        /// <summary>
        /// Tr?ng thái
        /// </summary>
        [StringLength(50)]
        public string? Status { get; set; }

        /// <summary>
        /// S? gi? ??c tính
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string? RequiredResources { get; set; }

        /// <summary>
        /// ID m?c tiêu cha
        /// </summary>
        public int? ParentGoalId { get; set; }

        /// <summary>
        /// Ph? thu?c vào
        /// </summary>
        public string? DependsOn { get; set; }

        /// <summary>
        /// Tr?ng thái ho?t ??ng
        /// </summary>
        public bool? IsActive { get; set; }
    }
}