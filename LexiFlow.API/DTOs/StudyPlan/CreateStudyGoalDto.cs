using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    /// <summary>
    /// DTO cho tạo mục tiêu học tập
    /// </summary>
    public class CreateStudyGoalDto
    {
        /// <summary>
        /// Tên mục tiêu
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GoalName { get; set; } = string.Empty;

        /// <summary>
        /// Loại mục tiêu
        /// </summary>
        [Required]
        [StringLength(50)]
        public string GoalType { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Giá trị mục tiêu
        /// </summary>
        [Required]
        public int TargetValue { get; set; }

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// ID cấp độ
        /// </summary>
        public int? LevelId { get; set; }

        /// <summary>
        /// ID chủ đề
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// Ngày mục tiêu
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Mức độ quan trọng (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Importance { get; set; } = 3;

        /// <summary>
        /// Mức độ khó (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3;

        /// <summary>
        /// Phạm vi mục tiêu
        /// </summary>
        [StringLength(50)]
        public string GoalScope { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng mục tiêu
        /// </summary>
        public int? TargetCount { get; set; }

        /// <summary>
        /// Đơn vị đo lường
        /// </summary>
        [StringLength(50)]
        public string MeasurementUnit { get; set; } = string.Empty;

        /// <summary>
        /// Tiêu chí thành công
        /// </summary>
        public string SuccessCriteria { get; set; } = string.Empty;

        /// <summary>
        /// Phương pháp xác minh
        /// </summary>
        public string VerificationMethod { get; set; } = string.Empty;

        /// <summary>
        /// Số giờ ước tính
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Nguồn lực cần thiết
        /// </summary>
        public string RequiredResources { get; set; } = string.Empty;

        /// <summary>
        /// ID mục tiêu cha
        /// </summary>
        public int? ParentGoalId { get; set; }

        /// <summary>
        /// Phụ thuộc vào
        /// </summary>
        public string DependsOn { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        public List<CreateStudyTaskDto> Tasks { get; set; } = new List<CreateStudyTaskDto>();
    }
}
