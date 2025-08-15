using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho c?p nh?t m?c ti�u h?c t?p
    /// </summary>
    public class UpdateStudyGoalDto
    {
        /// <summary>
        /// T�n m?c ti�u
        /// </summary>
        [StringLength(100)]
        public string? GoalName { get; set; }

        /// <summary>
        /// M� t?
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
        /// Ng�y m?c ti�u
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// M?c ?? quan tr?ng (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Importance { get; set; }

        /// <summary>
        /// M?c ?? kh� (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; }

        /// <summary>
        /// Lo?i m?c ti�u
        /// </summary>
        [StringLength(50)]
        public string? GoalType { get; set; }

        /// <summary>
        /// Ph?m vi m?c ti�u
        /// </summary>
        [StringLength(50)]
        public string? GoalScope { get; set; }

        /// <summary>
        /// S? l??ng m?c ti�u
        /// </summary>
        public int? TargetCount { get; set; }

        /// <summary>
        /// ??n v? ?o l??ng
        /// </summary>
        [StringLength(50)]
        public string? MeasurementUnit { get; set; }

        /// <summary>
        /// Ti�u ch� th�nh c�ng
        /// </summary>
        public string? SuccessCriteria { get; set; }

        /// <summary>
        /// Ph??ng ph�p x�c minh
        /// </summary>
        public string? VerificationMethod { get; set; }

        /// <summary>
        /// Tr?ng th�i
        /// </summary>
        [StringLength(50)]
        public string? Status { get; set; }

        /// <summary>
        /// S? gi? ??c t�nh
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Ngu?n l?c c?n thi?t
        /// </summary>
        public string? RequiredResources { get; set; }

        /// <summary>
        /// ID m?c ti�u cha
        /// </summary>
        public int? ParentGoalId { get; set; }

        /// <summary>
        /// Ph? thu?c v�o
        /// </summary>
        public string? DependsOn { get; set; }

        /// <summary>
        /// Tr?ng th�i ho?t ??ng
        /// </summary>
        public bool? IsActive { get; set; }
    }
}