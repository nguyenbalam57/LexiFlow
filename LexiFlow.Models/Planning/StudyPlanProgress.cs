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
    /// Tiến trình thực hiện kế hoạch học tập - theo dõi tiến độ của từng mục trong kế hoạch học
    /// Lưu trữ thông tin chi tiết về việc thực hiện các mục trong StudyPlanItem
    /// </summary>
    [Index(nameof(ItemId), nameof(UserId), Name = "IX_StudyPlanProgress_Item_User")]
    [Index(nameof(StudyPlanId), nameof(UserId), Name = "IX_StudyPlanProgress_Plan_User")]
    [Index(nameof(CompletedDate), Name = "IX_StudyPlanProgress_CompletedDate")]
    public class StudyPlanProgress : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của bản ghi tiến trình
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        /// <summary>
        /// ID của kế hoạch học tập
        /// </summary>
        [Required]
        public int StudyPlanId { get; set; }

        /// <summary>
        /// ID của người dùng thực hiện
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ID của mục trong kế hoạch học tập
        /// </summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// Phần trăm hoàn thành (0-100)
        /// </summary>
        [Range(0, 100)]
        public int CompletionPercentage { get; set; } = 0;

        /// <summary>
        /// Ngày hoàn thành thực tế
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Trạng thái hiện tại của mục
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "NotStarted"; // NotStarted, InProgress, Completed, Skipped, OnHold

        /// <summary>
        /// Thời gian thực tế dành cho mục này (phút)
        /// </summary>
        public int? ActualTimeMinutes { get; set; }

        /// <summary>
        /// Thời gian bắt đầu thực hiện mục
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Thời gian kết thúc thực hiện mục
        /// </summary>
        public DateTime? FinishedAt { get; set; }

        /// <summary>
        /// Mức độ khó của mục theo cảm nhận người dùng (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? PerceivedDifficulty { get; set; }

        /// <summary>
        /// Mức độ hài lòng với mục học tập (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? SatisfactionRating { get; set; }

        /// <summary>
        /// Mức độ hiệu quả của mục học tập (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? EffectivenessRating { get; set; }

        /// <summary>
        /// Ghi chú của người dùng về mục học tập
        /// </summary>
        public string UserNotes { get; set; }

        /// <summary>
        /// Thách thức gặp phải khi thực hiện mục
        /// </summary>
        public string EncounteredChallenges { get; set; }

        /// <summary>
        /// Đề xuất cải tiến cho mục học tập
        /// </summary>
        public string ImprovementSuggestions { get; set; }

        /// <summary>
        /// Số mục con đã hoàn thành (nếu có)
        /// </summary>
        public int? SubItemsCompleted { get; set; }

        /// <summary>
        /// Tổng số mục con (nếu có)
        /// </summary>
        public int? TotalSubItems { get; set; }

        /// <summary>
        /// Số câu trả lời đúng (nếu có bài test/quiz)
        /// </summary>
        public int? CorrectAnswers { get; set; }

        /// <summary>
        /// Tổng số câu hỏi (nếu có bài test/quiz)
        /// </summary>
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// Điểm số đạt được (nếu có)
        /// </summary>
        [Range(0, 100)]
        public float? AchievedScore { get; set; }

        /// <summary>
        /// Số lần thử lại mục học tập
        /// </summary>
        public int RetryCount { get; set; } = 0;

        /// <summary>
        /// Có phải lần thực hiện đầu tiên không
        /// </summary>
        public bool IsFirstAttempt { get; set; } = true;

        /// <summary>
        /// Thời gian dự kiến so với thực tế (phút)
        /// Positive = mất nhiều thời gian hơn dự kiến
        /// Negative = hoàn thành nhanh hơn dự kiến
        /// </summary>
        public int? TimeVarianceMinutes { get; set; }

        /// <summary>
        /// Mức độ khó dự kiến vs thực tế
        /// Positive = khó hơn dự kiến
        /// Negative = dễ hơn dự kiến
        /// </summary>
        public int? DifficultyVariance { get; set; }

        /// <summary>
        /// Dữ liệu tiến trình chi tiết (JSON format)
        /// Lưu trữ thông tin chi tiết về cách thực hiện mục
        /// </summary>
        public string ProgressDataJson { get; set; }

        /// <summary>
        /// Số liệu học tập chi tiết (JSON format)
        /// Bao gồm các metric về hiệu suất học tập
        /// </summary>
        public string LearningMetricsJson { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại tiến trình
        /// </summary>
        public string ProgressTags { get; set; }

        /// <summary>
        /// Có được đánh dấu là quan trọng không
        /// </summary>
        public bool IsImportant { get; set; } = false;

        /// <summary>
        /// Có cần xem xét lại không
        /// </summary>
        public bool NeedsReview { get; set; } = false;

        /// <summary>
        /// Có được đồng bộ với hệ thống khác không
        /// </summary>
        public bool IsSynced { get; set; } = false;

        /// <summary>
        /// Thời gian đồng bộ cuối cùng
        /// </summary>
        public DateTime? LastSyncedAt { get; set; }

        // Navigation properties
        /// <summary>
        /// Kế hoạch học tập
        /// </summary>
        [ForeignKey("StudyPlanId")]
        public virtual StudyPlan StudyPlan { get; set; }

        /// <summary>
        /// Người dùng thực hiện
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        /// <summary>
        /// Mục trong kế hoạch học tập
        /// </summary>
        [ForeignKey("ItemId")]
        public virtual StudyPlanItem StudyPlanItem { get; set; }

        // Computed Properties
        /// <summary>
        /// Tính toán tỷ lệ thành công (%)
        /// </summary>
        [NotMapped]
        public double SuccessRate 
        { 
            get 
            {
                if (TotalQuestions == null || TotalQuestions == 0) return 0;
                return CorrectAnswers.HasValue ? (double)CorrectAnswers.Value / TotalQuestions.Value * 100 : 0;
            }
        }

        /// <summary>
        /// Tính toán tỷ lệ hoàn thành sub-items (%)
        /// </summary>
        [NotMapped]
        public double SubItemCompletionRate
        {
            get
            {
                if (TotalSubItems == null || TotalSubItems == 0) return 0;
                return SubItemsCompleted.HasValue ? (double)SubItemsCompleted.Value / TotalSubItems.Value * 100 : 0;
            }
        }

        /// <summary>
        /// Tính toán thời gian thực hiện (phút)
        /// </summary>
        [NotMapped]
        public int CalculatedDurationMinutes
        {
            get
            {
                if (StartedAt.HasValue && FinishedAt.HasValue)
                {
                    return (int)(FinishedAt.Value - StartedAt.Value).TotalMinutes;
                }
                return ActualTimeMinutes ?? 0;
            }
        }

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

        // Methods
        /// <summary>
        /// Bắt đầu thực hiện mục học tập
        /// </summary>
        public virtual void StartProgress()
        {
            Status = "InProgress";
            StartedAt = DateTime.UtcNow;
            if (CompletionPercentage == 0)
                CompletionPercentage = 1; // Đánh dấu đã bắt đầu
            UpdateTimestamp();
        }

        /// <summary>
        /// Đánh dấu mục là hoàn thành
        /// </summary>
        /// <param name="completedBy">ID người hoàn thành</param>
        /// <param name="notes">Ghi chú hoàn thành</param>
        public virtual void MarkCompleted(int completedBy, string notes = null)
        {
            Status = "Completed";
            CompletionPercentage = 100;
            CompletedDate = DateTime.UtcNow;
            FinishedAt = DateTime.UtcNow;
            
            if (!string.IsNullOrEmpty(notes))
                UserNotes = notes;

            if (StartedAt.HasValue)
                ActualTimeMinutes = (int)(DateTime.UtcNow - StartedAt.Value).TotalMinutes;

            UpdateModification(completedBy, "Đánh dấu hoàn thành mục học tập");
        }

        /// <summary>
        /// Cập nhật tiến độ
        /// </summary>
        /// <param name="percentage">Phần trăm hoàn thành mới</param>
        /// <param name="updatedBy">ID người cập nhật</param>
        public virtual void UpdateProgress(int percentage, int updatedBy)
        {
            CompletionPercentage = Math.Max(0, Math.Min(100, percentage));
            
            if (CompletionPercentage >= 100)
            {
                MarkCompleted(updatedBy);
            }
            else if (CompletionPercentage > 0 && Status == "NotStarted")
            {
                StartProgress();
            }

            UpdateModification(updatedBy, $"Cập nhật tiến độ: {percentage}%");
        }

        /// <summary>
        /// Thêm đánh giá và phản hồi
        /// </summary>
        /// <param name="difficulty">Mức độ khó (1-5)</param>
        /// <param name="satisfaction">Mức độ hài lòng (1-5)</param>
        /// <param name="effectiveness">Mức độ hiệu quả (1-5)</param>
        /// <param name="notes">Ghi chú</param>
        public virtual void AddFeedback(int? difficulty, int? satisfaction, int? effectiveness, string notes = null)
        {
            PerceivedDifficulty = difficulty;
            SatisfactionRating = satisfaction;
            EffectivenessRating = effectiveness;
            
            if (!string.IsNullOrEmpty(notes))
                UserNotes = string.IsNullOrEmpty(UserNotes) ? notes : $"{UserNotes}\n{notes}";
                
            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật kết quả test/quiz
        /// </summary>
        /// <param name="correct">Số câu đúng</param>
        /// <param name="total">Tổng số câu</param>
        /// <param name="score">Điểm số</param>
        public virtual void UpdateTestResults(int correct, int total, float? score = null)
        {
            CorrectAnswers = correct;
            TotalQuestions = total;
            AchievedScore = score ?? (float)SuccessRate;
            UpdateTimestamp();
        }

        /// <summary>
        /// Tạm dừng thực hiện mục
        /// </summary>
        /// <param name="reason">Lý do tạm dừng</param>
        public virtual void PauseProgress(string reason = null)
        {
            Status = "OnHold";
            if (!string.IsNullOrEmpty(reason))
                UserNotes = string.IsNullOrEmpty(UserNotes) ? $"Tạm dừng: {reason}" : $"{UserNotes}\nTạm dừng: {reason}";
            UpdateTimestamp();
        }

        /// <summary>
        /// Bỏ qua mục học tập
        /// </summary>
        /// <param name="reason">Lý do bỏ qua</param>
        /// <param name="skippedBy">ID người bỏ qua</param>
        public virtual void SkipProgress(string reason, int skippedBy)
        {
            Status = "Skipped";
            CompletionPercentage = 0;
            UserNotes = string.IsNullOrEmpty(UserNotes) ? $"Bỏ qua: {reason}" : $"{UserNotes}\nBỏ qua: {reason}";
            UpdateModification(skippedBy, $"Bỏ qua mục học tập: {reason}");
        }

        /// <summary>
        /// Lấy tên hiển thị của tiến trình
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return $"Tiến trình {StudyPlanItem?.ItemName ?? "Mục học tập"} - {CompletionPercentage}%";
        }

        /// <summary>
        /// Validate tiến trình
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && StudyPlanId > 0 
                && UserId > 0 
                && ItemId > 0
                && CompletionPercentage >= 0 && CompletionPercentage <= 100
                && (!TotalQuestions.HasValue || (CorrectAnswers.HasValue && CorrectAnswers.Value <= TotalQuestions.Value))
                && (!TotalSubItems.HasValue || (SubItemsCompleted.HasValue && SubItemsCompleted.Value <= TotalSubItems.Value));
        }
    }
}
