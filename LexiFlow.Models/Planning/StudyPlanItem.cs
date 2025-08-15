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
    /// Mục trong kế hoạch học tập - định nghĩa các thành phần cụ thể trong một kế hoạch học
    /// Có thể là học từ vựng, ngữ pháp, kanji, làm bài test, v.v.
    /// </summary>
    [Index(nameof(PlanId), nameof(ItemType), Name = "IX_StudyPlanItem_Plan_Type")]
    [Index(nameof(ScheduledDate), Name = "IX_StudyPlanItem_ScheduledDate")]
    [Index(nameof(Priority), Name = "IX_StudyPlanItem_Priority")]
    public class StudyPlanItem : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của mục trong kế hoạch học tập
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// ID của kế hoạch học tập chứa mục này
        /// </summary>
        [Required]
        public int PlanId { get; set; }

        /// <summary>
        /// Tên của mục học tập
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ItemName { get; set; }

        /// <summary>
        /// Loại mục học tập (Vocabulary, Grammar, Kanji, Reading, Listening, Test, etc.)
        /// </summary>
        [StringLength(50)]
        public string ItemType { get; set; }

        /// <summary>
        /// Nội dung chi tiết của mục học tập
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Mô tả về mục học tập
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ngày dự kiến thực hiện
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Ngày đến hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Mức độ ưu tiên (1-5, 5 là ưu tiên cao nhất)
        /// </summary>
        [Range(1, 5)]
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Thời gian ước tính cần thiết
        /// </summary>
        public int? EstimatedTimeMinutes { get; set; }

        /// <summary>
        /// Thời gian thực tế đã dành (phút)
        /// </summary>
        public int? ActualTimeMinutes { get; set; }

        /// <summary>
        /// ID chủ đề học tập (nếu có)
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// ID nhóm từ vựng (nếu là học từ vựng)
        /// </summary>
        public int? VocabularyGroupId { get; set; }

        /// <summary>
        /// ID điểm ngữ pháp (nếu là học ngữ pháp)
        /// </summary>
        public int? GrammarId { get; set; }

        /// <summary>
        /// ID bài kiểm tra (nếu là làm bài test)
        /// </summary>
        public int? ExamId { get; set; }

        /// <summary>
        /// ID danh mục liên quan
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Có phải mục lặp lại không
        /// </summary>
        public bool IsRecurring { get; set; } = false;

        /// <summary>
        /// Mẫu lặp lại (Daily, Weekly, Monthly, etc.)
        /// </summary>
        [StringLength(50)]
        public string RecurrencePattern { get; set; }

        /// <summary>
        /// Số lần lặp lại (-1 = vô hạn)
        /// </summary>
        public int? RecurrenceCount { get; set; }

        /// <summary>
        /// Có phải mục bắt buộc không
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// Trạng thái hiện tại của mục
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "Planned"; // Planned, InProgress, Completed, Skipped, OnHold

        /// <summary>
        /// Phần trăm hoàn thành (0-100)
        /// </summary>
        [Range(0, 100)]
        public float CompletionPercentage { get; set; } = 0;

        /// <summary>
        /// Thời gian hoàn thành thực tế
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Ghi chú về mục học tập
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Có được đánh dấu nổi bật không
        /// </summary>
        public bool IsHighlighted { get; set; } = false;

        /// <summary>
        /// Có được đánh dấu yêu thích không
        /// </summary>
        public bool IsFavorite { get; set; } = false;

        /// <summary>
        /// Danh sách ID các mục phụ thuộc (JSON format)
        /// </summary>
        public string DependenciesJson { get; set; }

        /// <summary>
        /// Yêu cầu để hoàn thành mục
        /// </summary>
        public string CompletionRequirements { get; set; }

        /// <summary>
        /// Điều kiện để bắt đầu mục
        /// </summary>
        public string StartConditions { get; set; }

        /// <summary>
        /// Mục tiêu cụ thể cần đạt được
        /// </summary>
        public string LearningObjectives { get; set; }

        /// <summary>
        /// Phương pháp đánh giá
        /// </summary>
        public string AssessmentMethod { get; set; }

        /// <summary>
        /// Tài liệu tham khảo (JSON format)
        /// </summary>
        public string ResourcesJson { get; set; }

        /// <summary>
        /// URL tài liệu đính kèm (JSON format)
        /// </summary>
        public string AttachmentsJson { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại (JSON format)
        /// </summary>
        public string TagsJson { get; set; }

        /// <summary>
        /// Mức độ khó dự kiến (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? ExpectedDifficulty { get; set; }

        /// <summary>
        /// Mức độ khó thực tế (1-5) - được cập nhật sau khi hoàn thành
        /// </summary>
        [Range(1, 5)]
        public int? ActualDifficulty { get; set; }

        /// <summary>
        /// Điểm số đạt được (nếu có)
        /// </summary>
        [Range(0, 100)]
        public float? AchievedScore { get; set; }

        /// <summary>
        /// Điểm số mục tiêu
        /// </summary>
        [Range(0, 100)]
        public float? TargetScore { get; set; }

        /// <summary>
        /// Số lần thử lại
        /// </summary>
        public int RetryCount { get; set; } = 0;

        /// <summary>
        /// Có cho phép thử lại không
        /// </summary>
        public bool AllowRetry { get; set; } = true;

        /// <summary>
        /// Số lần thử lại tối đa
        /// </summary>
        public int? MaxRetries { get; set; }

        /// <summary>
        /// Có tự động chuyển sang mục tiếp theo không
        /// </summary>
        public bool AutoAdvance { get; set; } = false;

        /// <summary>
        /// Có cần xem xét lại không
        /// </summary>
        public bool NeedsReview { get; set; } = false;

        /// <summary>
        /// Thời gian xem xét lại tiếp theo
        /// </summary>
        public DateTime? NextReviewDate { get; set; }

        /// <summary>
        /// Khoảng thời gian xem xét lại (ngày)
        /// </summary>
        public int? ReviewIntervalDays { get; set; }

        /// <summary>
        /// Cài đặt nhắc nhở (JSON format)
        /// </summary>
        public string ReminderSettings { get; set; }

        /// <summary>
        /// Có được đồng bộ với hệ thống khác không
        /// </summary>
        public bool IsSynced { get; set; } = false;

        /// <summary>
        /// Thời gian đồng bộ cuối cùng
        /// </summary>
        public DateTime? LastSyncedAt { get; set; }

        /// <summary>
        /// ID nguồn tạo ra mục (Manual, Auto, Import, etc.)
        /// </summary>
        public string SourceType { get; set; } = "Manual";

        /// <summary>
        /// ID từ hệ thống bên ngoài (nếu được import)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Metadata bổ sung (JSON format)
        /// </summary>
        public string MetadataJson { get; set; }

        // Navigation properties
        /// <summary>
        /// Kế hoạch học tập chứa mục này
        /// </summary>
        [ForeignKey("PlanId")]
        public virtual StudyPlan Plan { get; set; }

        /// <summary>
        /// Chủ đề học tập (nếu có)
        /// </summary>
        [ForeignKey("TopicId")]
        public virtual StudyTopic Topic { get; set; }

        /// <summary>
        /// Nhóm từ vựng (nếu có)
        /// </summary>
        [ForeignKey("VocabularyGroupId")]
        public virtual Learning.Vocabulary.VocabularyGroup VocabularyGroup { get; set; }

        /// <summary>
        /// Điểm ngữ pháp (nếu có)
        /// </summary>
        [ForeignKey("GrammarId")]
        public virtual Learning.Grammar.Grammar Grammar { get; set; }

        /// <summary>
        /// Bài kiểm tra (nếu có)
        /// </summary>
        [ForeignKey("ExamId")]
        public virtual Exam.JLPTExam Exam { get; set; }

        /// <summary>
        /// Danh mục (nếu có)
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Learning.Vocabulary.Category Category { get; set; }

        /// <summary>
        /// Danh sách tiến trình thực hiện
        /// </summary>
        public virtual ICollection<StudyPlanProgress> StudyPlanProgresses { get; set; }

        /// <summary>
        /// Danh sách nhiệm vụ thuộc mục này
        /// </summary>
        public virtual ICollection<StudyTask> StudyTasks { get; set; }

        // Computed Properties
        /// <summary>
        /// Kiểm tra xem có hoàn thành không
        /// </summary>
        [NotMapped]
        public bool IsCompleted => CompletionPercentage >= 100 && Status == "Completed";

        /// <summary>
        /// Kiểm tra xem có đang thực hiện không
        /// </summary>
        [NotMapped]
        public bool IsInProgress => Status == "InProgress" && CompletionPercentage > 0 && CompletionPercentage < 100;

        /// <summary>
        /// Kiểm tra xem có quá hạn không
        /// </summary>
        [NotMapped]
        public bool IsOverdue => DueDate.HasValue && DateTime.UtcNow > DueDate.Value && !IsCompleted;

        /// <summary>
        /// Kiểm tra xem có sắp đến hạn không (trong vòng 24h)
        /// </summary>
        [NotMapped]
        public bool IsDueSoon => DueDate.HasValue && DateTime.UtcNow.AddHours(24) >= DueDate.Value && !IsCompleted;

        /// <summary>
        /// Tính toán hiệu suất (achieved vs target score)
        /// </summary>
        [NotMapped]
        public float? PerformanceRatio
        {
            get
            {
                if (AchievedScore.HasValue && TargetScore.HasValue && TargetScore.Value > 0)
                    return AchievedScore.Value / TargetScore.Value;
                return null;
            }
        }

        // Methods
        /// <summary>
        /// Bắt đầu thực hiện mục
        /// </summary>
        /// <param name="startedBy">ID người bắt đầu</param>
        public virtual void Start(int startedBy)
        {
            Status = "InProgress";
            if (CompletionPercentage == 0)
                CompletionPercentage = 1; // Đánh dấu đã bắt đầu
            UpdateModification(startedBy, "Bắt đầu thực hiện mục học tập");
        }

        /// <summary>
        /// Đánh dấu mục là hoàn thành
        /// </summary>
        /// <param name="completedBy">ID người hoàn thành</param>
        /// <param name="score">Điểm số đạt được</param>
        /// <param name="notes">Ghi chú hoàn thành</param>
        public virtual void MarkCompleted(int completedBy, float? score = null, string notes = null)
        {
            Status = "Completed";
            CompletionPercentage = 100;
            CompletedAt = DateTime.UtcNow;
            
            if (score.HasValue)
                AchievedScore = score.Value;
                
            if (!string.IsNullOrEmpty(notes))
                Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}\n{notes}";

            if (ScheduledDate.HasValue)
                ActualTimeMinutes = (int)(DateTime.UtcNow - ScheduledDate.Value).TotalMinutes;

            UpdateModification(completedBy, "Đánh dấu hoàn thành mục học tập");
        }

        /// <summary>
        /// Cập nhật tiến độ
        /// </summary>
        /// <param name="percentage">Phần trăm hoàn thành mới</param>
        /// <param name="updatedBy">ID người cập nhật</param>
        public virtual void UpdateProgress(float percentage, int updatedBy)
        {
            CompletionPercentage = Math.Max(0, Math.Min(100, percentage));
            
            if (CompletionPercentage >= 100)
            {
                MarkCompleted(updatedBy);
            }
            else if (CompletionPercentage > 0 && Status == "Planned")
            {
                Start(updatedBy);
            }

            UpdateModification(updatedBy, $"Cập nhật tiến độ: {percentage}%");
        }

        /// <summary>
        /// Bỏ qua mục học tập
        /// </summary>
        /// <param name="reason">Lý do bỏ qua</param>
        /// <param name="skippedBy">ID người bỏ qua</param>
        public virtual void Skip(string reason, int skippedBy)
        {
            Status = "Skipped";
            CompletionPercentage = 0;
            Notes = string.IsNullOrEmpty(Notes) ? $"Bỏ qua: {reason}" : $"{Notes}\nBỏ qua: {reason}";
            UpdateModification(skippedBy, $"Bỏ qua mục học tập: {reason}");
        }

        /// <summary>
        /// Tạm dừng mục học tập
        /// </summary>
        /// <param name="reason">Lý do tạm dừng</param>
        /// <param name="pausedBy">ID người tạm dừng</param>
        public virtual void Pause(string reason, int pausedBy)
        {
            Status = "OnHold";
            Notes = string.IsNullOrEmpty(Notes) ? $"Tạm dừng: {reason}" : $"{Notes}\nTạm dừng: {reason}";
            UpdateModification(pausedBy, $"Tạm dừng mục học tập: {reason}");
        }

        /// <summary>
        /// Thử lại mục học tập
        /// </summary>
        /// <param name="retriedBy">ID người thử lại</param>
        public virtual void Retry(int retriedBy)
        {
            if (!AllowRetry || (MaxRetries.HasValue && RetryCount >= MaxRetries.Value))
                return;

            RetryCount++;
            CompletionPercentage = 0;
            Status = "Planned";
            AchievedScore = null;
            CompletedAt = null;
            
            UpdateModification(retriedBy, $"Thử lại mục học tập (lần {RetryCount})");
        }

        /// <summary>
        /// Lên lịch xem xét lại
        /// </summary>
        /// <param name="intervalDays">Khoảng cách ngày xem xét lại</param>
        public virtual void ScheduleReview(int intervalDays)
        {
            ReviewIntervalDays = intervalDays;
            NextReviewDate = DateTime.UtcNow.AddDays(intervalDays);
            NeedsReview = true;
            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật đánh giá độ khó
        /// </summary>
        /// <param name="difficulty">Mức độ khó thực tế (1-5)</param>
        /// <param name="reviewedBy">ID người đánh giá</param>
        public virtual void UpdateDifficultyRating(int difficulty, int reviewedBy)
        {
            if (difficulty >= 1 && difficulty <= 5)
            {
                ActualDifficulty = difficulty;
                UpdateModification(reviewedBy, $"Cập nhật mức độ khó: {difficulty}/5");
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của mục
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return ItemName;
        }

        /// <summary>
        /// Validate mục học tập
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && !string.IsNullOrWhiteSpace(ItemName)
                && PlanId > 0
                && Priority >= 1 && Priority <= 5
                && CompletionPercentage >= 0 && CompletionPercentage <= 100
                && (!DueDate.HasValue || !ScheduledDate.HasValue || ScheduledDate.Value <= DueDate.Value)
                && (!MaxRetries.HasValue || MaxRetries.Value >= 0)
                && (!ExpectedDifficulty.HasValue || (ExpectedDifficulty >= 1 && ExpectedDifficulty <= 5))
                && (!ActualDifficulty.HasValue || (ActualDifficulty >= 1 && ActualDifficulty <= 5))
                && (!AchievedScore.HasValue || (AchievedScore >= 0 && AchievedScore <= 100))
                && (!TargetScore.HasValue || (TargetScore >= 0 && TargetScore <= 100));
        }
    }
}
