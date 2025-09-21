using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Planning
{
    /// <summary>
    /// Mục tiêu học tập - định nghĩa các mục tiêu cụ thể trong kế hoạch học
    /// Mỗi mục tiêu có thể được chia nhỏ thành các nhiệm vụ (StudyTask)
    /// </summary>
    [Index(nameof(PlanId), nameof(GoalName), IsUnique = true, Name = "IX_StudyGoal_Plan_Name")]
    public class StudyGoal : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của mục tiêu học tập
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }

        /// <summary>
        /// ID của kế hoạch học tập chứa mục tiêu này
        /// </summary>
        [Required]
        public int PlanId { get; set; }

        /// <summary>
        /// Tên của mục tiêu học tập
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GoalName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về mục tiêu
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ID cấp độ JLPT (nếu áp dụng)
        /// </summary>
        public int? LevelId { get; set; }

        /// <summary>
        /// ID chủ đề học tập (nếu áp dụng)
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// ID danh mục (nếu áp dụng)
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Ngày mục tiêu dự kiến hoàn thành
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Mức độ quan trọng của mục tiêu (1-5, 5 là quan trọng nhất)
        /// </summary>
        [Range(1, 5)]
        public int? Importance { get; set; } = 3;

        /// <summary>
        /// Mức độ khó của mục tiêu (1-5, 5 là khó nhất)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3;

        /// <summary>
        /// Loại mục tiêu (Vocabulary, Grammar, Reading, Listening, etc.)
        /// </summary>
        [StringLength(50)]
        public string GoalType { get; set; }

        /// <summary>
        /// Phạm vi mục tiêu (Specific, General, Milestone)
        /// </summary>
        [StringLength(50)]
        public string GoalScope { get; set; }

        /// <summary>
        /// Số lượng mục tiêu cần đạt được
        /// </summary>
        public int? TargetCount { get; set; }

        /// <summary>
        /// Đơn vị đo lường (Words, Hours, Points, etc.)
        /// </summary>
        [StringLength(50)]
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Tiêu chí đánh giá thành công
        /// </summary>
        public string SuccessCriteria { get; set; }

        /// <summary>
        /// Phương pháp xác minh kết quả
        /// </summary>
        public string VerificationMethod { get; set; }

        /// <summary>
        /// Trạng thái hoàn thành của mục tiêu
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Ngày hoàn thành thực tế
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Phần trăm tiến độ hoàn thành (0-100)
        /// </summary>
        [Range(0, 100)]
        public float ProgressPercentage { get; set; } = 0;

        /// <summary>
        /// Trạng thái hiện tại của mục tiêu
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "NotStarted"; // NotStarted, InProgress, OnHold, Completed, Cancelled

        /// <summary>
        /// Số giờ ước tính cần để hoàn thành mục tiêu
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Số giờ thực tế đã dành cho mục tiêu
        /// </summary>
        public int? ActualHours { get; set; }

        /// <summary>
        /// Nguồn lực cần thiết để thực hiện mục tiêu
        /// </summary>
        public string RequiredResources { get; set; }

        /// <summary>
        /// ID mục tiêu cha (nếu là mục tiêu con)
        /// </summary>
        public int? ParentGoalId { get; set; }

        /// <summary>
        /// Danh sách ID các mục tiêu mà mục tiêu này phụ thuộc vào
        /// </summary>
        public string DependsOn { get; set; }

        /// <summary>
        /// Ghi chú về mục tiêu
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại mục tiêu
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Độ ưu tiên (Higher number = Higher priority)
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// Có tự động cập nhật tiến độ hay không
        /// </summary>
        public bool AutoUpdateProgress { get; set; } = true;

        // Navigation properties
        /// <summary>
        /// Kế hoạch học tập chứa mục tiêu này
        /// </summary>
        [ForeignKey("PlanId")]
        public virtual StudyPlan StudyPlan { get; set; }

        /// <summary>
        /// Cấp độ JLPT (nếu áp dụng)
        /// </summary>
        [ForeignKey("LevelId")]
        public virtual Exam.JLPTLevel Level { get; set; }

        /// <summary>
        /// Chủ đề học tập (nếu áp dụng)
        /// </summary>
        [ForeignKey("TopicId")]
        public virtual StudyTopic Topic { get; set; }

        /// <summary>
        /// Danh mục (nếu áp dụng)
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Learning.Vocabulary.Category Category { get; set; }

        /// <summary>
        /// Mục tiêu cha (nếu là mục tiêu con)
        /// </summary>
        [ForeignKey("ParentGoalId")]
        public virtual StudyGoal ParentGoal { get; set; }

        /// <summary>
        /// Danh sách các nhiệm vụ học tập thuộc mục tiêu này
        /// </summary>
        public virtual ICollection<StudyTask> StudyTasks { get; set; }

        /// <summary>
        /// Danh sách các mục tiêu con
        /// </summary>
        public virtual ICollection<StudyGoal> ChildGoals { get; set; }

        /// <summary>
        /// Danh sách tiến trình mục tiêu
        /// </summary>
        public virtual ICollection<Progress.GoalProgress> GoalProgresses { get; set; }

        // Methods
        /// <summary>
        /// Tính toán phần trăm hoàn thành dựa trên các nhiệm vụ con
        /// </summary>
        /// <returns>Phần trăm hoàn thành</returns>
        public virtual float CalculateProgressPercentage()
        {
            if (StudyTasks == null || !StudyTasks.Any())
                return ProgressPercentage;

            var totalTasks = StudyTasks.Count();
            var completedTasks = StudyTasks.Count(t => t.IsCompleted);
            return totalTasks > 0 ? (float)completedTasks / totalTasks * 100 : 0;
        }

        /// <summary>
        /// Kiểm tra xem mục tiêu có quá hạn không
        /// </summary>
        /// <returns>True nếu quá hạn</returns>
        public virtual bool IsOverdue()
        {
            return TargetDate.HasValue && DateTime.UtcNow > TargetDate.Value && !IsCompleted;
        }

        /// <summary>
        /// Đánh dấu mục tiêu là hoàn thành
        /// </summary>
        /// <param name="completedBy">ID người hoàn thành</param>
        public virtual void MarkAsCompleted(int completedBy)
        {
            IsCompleted = true;
            CompletedDate = DateTime.UtcNow;
            Status = "Completed";
            ProgressPercentage = 100;
            UpdateModification(completedBy, "Mục tiêu đã hoàn thành");
        }

        /// <summary>
        /// Cập nhật tiến độ mục tiêu
        /// </summary>
        /// <param name="percentage">Phần trăm tiến độ mới</param>
        /// <param name="updatedBy">ID người cập nhật</param>
        public virtual void UpdateProgress(float percentage, int updatedBy)
        {
            ProgressPercentage = Math.Max(0, Math.Min(100, percentage));
            
            if (ProgressPercentage >= 100 && !IsCompleted)
            {
                MarkAsCompleted(updatedBy);
            }
            else if (ProgressPercentage > 0 && Status == "NotStarted")
            {
                Status = "InProgress";
            }

            UpdateModification(updatedBy, $"Cập nhật tiến độ: {percentage}%");
        }

        /// <summary>
        /// Lấy tên hiển thị của mục tiêu
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return GoalName;
        }

        /// <summary>
        /// Validate mục tiêu
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && !string.IsNullOrWhiteSpace(GoalName)
                && PlanId > 0
                && ProgressPercentage >= 0 && ProgressPercentage <= 100;
        }
    }
}
