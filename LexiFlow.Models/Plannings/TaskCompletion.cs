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
    /// Thông tin hoàn thành nhiệm vụ học tập - lưu trữ chi tiết về việc thực hiện các nhiệm vụ
    /// Bao gồm thông tin về thời gian, hiệu quả, điểm số và phản hồi
    /// </summary>
    [Index(nameof(TaskId), nameof(CompletionDate), Name = "IX_TaskCompletion_Task_Date")]
    [Index(nameof(UserId), nameof(CompletionDate), Name = "IX_TaskCompletion_User_Date")]
    public class TaskCompletion : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của bản ghi hoàn thành nhiệm vụ
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompletionId { get; set; }

        /// <summary>
        /// ID của nhiệm vụ được hoàn thành
        /// </summary>
        [Required]
        public int TaskId { get; set; }

        /// <summary>
        /// ID của người dùng hoàn thành nhiệm vụ
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ID của phiên học tập (nếu có)
        /// </summary>
        public int? SessionId { get; set; }

        /// <summary>
        /// Thời gian hoàn thành nhiệm vụ
        /// </summary>
        [Required]
        public DateTime CompletionDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Phần trăm hoàn thành (0-100)
        /// </summary>
        [Range(0, 100)]
        public int CompletionPercentage { get; set; } = 100;

        /// <summary>
        /// Thời gian thực tế dành cho nhiệm vụ (phút)
        /// </summary>
        public int? ActualDurationMinutes { get; set; }

        /// <summary>
        /// Mức độ khó của nhiệm vụ theo cảm nhận người dùng (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? PerceivedDifficulty { get; set; }

        /// <summary>
        /// Mức độ hài lòng với nhiệm vụ (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? SatisfactionRating { get; set; }

        /// <summary>
        /// Mức độ hiệu quả của nhiệm vụ (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? EffectivenessRating { get; set; }

        /// <summary>
        /// Ghi chú của người dùng về nhiệm vụ
        /// </summary>
        public string UserNotes { get; set; }

        /// <summary>
        /// Phản hồi chi tiết về nhiệm vụ
        /// </summary>
        public string DetailedFeedback { get; set; }

        /// <summary>
        /// Kết quả học tập đạt được
        /// </summary>
        public string LearningOutcome { get; set; }

        /// <summary>
        /// Điểm số đạt được (nếu có)
        /// </summary>
        [Range(0, 100)]
        public int? AchievedScore { get; set; }

        /// <summary>
        /// Số câu trả lời đúng (nếu có)
        /// </summary>
        public int? CorrectAnswers { get; set; }

        /// <summary>
        /// Tổng số câu hỏi (nếu có)
        /// </summary>
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// Dữ liệu hoàn thành chi tiết (JSON format)
        /// Lưu trữ thông tin cụ thể về cách hoàn thành nhiệm vụ
        /// </summary>
        public string CompletionDataJson { get; set; }

        /// <summary>
        /// Số liệu học tập chi tiết (JSON format)
        /// Bao gồm các metric về hiệu suất học tập
        /// </summary>
        public string LearningMetricsJson { get; set; }

        /// <summary>
        /// ID của thực thể liên quan (vocabulary, kanji, grammar, v.v.)
        /// </summary>
        public int? RelatedContentId { get; set; }

        /// <summary>
        /// Loại nội dung liên quan (Vocabulary, Kanji, Grammar, v.v.)
        /// </summary>
        [StringLength(50)]
        public string RelatedContentType { get; set; }

        /// <summary>
        /// Thời gian bắt đầu thực hiện nhiệm vụ
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Thời gian kết thúc thực hiện nhiệm vụ
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Số lần thử lại nhiệm vụ
        /// </summary>
        public int RetryCount { get; set; } = 0;

        /// <summary>
        /// Có phải là lần hoàn thành đầu tiên không
        /// </summary>
        public bool IsFirstAttempt { get; set; } = true;

        /// <summary>
        /// Độ khó thực tế so với dự kiến (difference between perceived and estimated)
        /// </summary>
        public int? DifficultyVariance { get; set; }

        /// <summary>
        /// Thời gian thực tế so với dự kiến (phút)
        /// </summary>
        public int? TimeVariance { get; set; }

        /// <summary>
        /// Trạng thái hoàn thành chi tiết
        /// </summary>
        [StringLength(50)]
        public string CompletionStatus { get; set; } = "Completed"; // Completed, PartiallyCompleted, Failed, Skipped

        /// <summary>
        /// Thẻ tag để phân loại hoàn thành
        /// </summary>
        public string CompletionTags { get; set; }

        // Navigation properties
        /// <summary>
        /// Nhiệm vụ được hoàn thành
        /// </summary>
        [ForeignKey("TaskId")]
        public virtual StudyTask StudyTask { get; set; }

        /// <summary>
        /// Người dùng hoàn thành nhiệm vụ
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        /// <summary>
        /// Phiên học tập liên quan (nếu có)
        /// </summary>
        [ForeignKey("SessionId")]
        public virtual StudySession StudySession { get; set; }

        // Computed properties
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
        /// Tính toán thời gian thực hiện (phút)
        /// </summary>
        [NotMapped]
        public int CalculatedDuration
        {
            get
            {
                if (StartTime.HasValue && EndTime.HasValue)
                {
                    return (int)(EndTime.Value - StartTime.Value).TotalMinutes;
                }
                return ActualDurationMinutes ?? 0;
            }
        }

        // Methods
        /// <summary>
        /// Đánh dấu nhiệm vụ là hoàn thành
        /// </summary>
        /// <param name="completionPercentage">Phần trăm hoàn thành</param>
        /// <param name="actualMinutes">Thời gian thực tế (phút)</param>
        public virtual void MarkCompleted(int completionPercentage = 100, int? actualMinutes = null)
        {
            CompletionPercentage = Math.Max(0, Math.Min(100, completionPercentage));
            CompletionDate = DateTime.UtcNow;
            EndTime = DateTime.UtcNow;
            
            if (actualMinutes.HasValue)
                ActualDurationMinutes = actualMinutes.Value;
            else if (StartTime.HasValue)
                ActualDurationMinutes = (int)(DateTime.UtcNow - StartTime.Value).TotalMinutes;

            CompletionStatus = completionPercentage >= 100 ? "Completed" : "PartiallyCompleted";
            UpdateTimestamp();
        }

        /// <summary>
        /// Bắt đầu thực hiện nhiệm vụ
        /// </summary>
        public virtual void StartTask()
        {
            StartTime = DateTime.UtcNow;
            CompletionStatus = "InProgress";
            UpdateTimestamp();
        }

        /// <summary>
        /// Thêm phản hồi và đánh giá
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
            UserNotes = notes;
            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật kết quả bài test/quiz
        /// </summary>
        /// <param name="correct">Số câu đúng</param>
        /// <param name="total">Tổng số câu</param>
        /// <param name="score">Điểm số</param>
        public virtual void UpdateTestResults(int correct, int total, int? score = null)
        {
            CorrectAnswers = correct;
            TotalQuestions = total;
            AchievedScore = score ?? (int)(SuccessRate);
            UpdateTimestamp();
        }

        /// <summary>
        /// Lấy tên hiển thị của completion
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return $"Hoàn thành {StudyTask?.TaskName ?? "Nhiệm vụ"} - {CompletionDate:dd/MM/yyyy}";
        }

        /// <summary>
        /// Validate completion
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && TaskId > 0 
                && UserId > 0
                && CompletionPercentage >= 0 && CompletionPercentage <= 100
                && (!TotalQuestions.HasValue || (CorrectAnswers.HasValue && CorrectAnswers.Value <= TotalQuestions.Value));
        }
    }
}
