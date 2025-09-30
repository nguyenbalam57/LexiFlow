using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Exams
{
    /// <summary>
    /// Câu hỏi thi với các cải tiến về performance và audit
    /// </summary>
    [Index(nameof(QuestionType), Name = "IX_Question_Type")]
    [Index(nameof(CreatedBy), Name = "IX_Question_CreatedBy")]
    [Index(nameof(IsActive), nameof(QuestionType), Name = "IX_Question_Active_Type")]
    [Index(nameof(IsDeleted), Name = "IX_Question_SoftDelete")]
    [Index(nameof(Difficulty), Name = "IX_Question_Difficulty")]
    [Index(nameof(Tags), Name = "IX_Question_Tags")] // For full-text search
    public class Question : AuditableEntity
    {
        /// <summary>
        /// Id câu hỏi (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        /// <summary>
        /// Loại câu hỏi (VD: Multiple Choice, Fill in the Blank, True/False)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string QuestionType { get; set; } = QuestionTypeStatics.MULTIPLE_CHOICE;

        /// <summary>
        /// Nội dung câu hỏi
        /// </summary>
        [Required]
        public string QuestionText { get; set; }

        /// <summary>
        /// Hướng dẫn câu hỏi (nếu có)
        /// </summary>
        public string QuestionInstruction { get; set; }

        /// <summary>
        /// Giải thích chi tiết về câu hỏi (nếu có)
        /// </summary>
        public string QuestionExplanation { get; set; }

        /// <summary>
        /// Trình độ khó (VD: Easy, Medium, Hard)
        /// </summary>
        [StringLength(50)]
        public string Difficulty { get; set; } = "Medium";

        /// <summary>
        /// Điểm số cho câu hỏi
        /// </summary>
        [Range(1, 100)]
        public int Points { get; set; } = 1;

        /// <summary>
        /// Thời gian giới hạn để trả lời câu hỏi (giây)
        /// Tính bằng giây
        /// </summary>
        [Range(1, 3600)] // Max 1 hour per question
        public int TimeLimit { get; set; } = 60;

        /// <summary>
        /// Tags để phân loại và tìm kiếm câu hỏi
        /// </summary>
        [StringLength(500)]
        public string Tags { get; set; }

        // Performance tracking

        /// <summary>
        /// Số lần câu hỏi đã được sử dụng
        /// </summary>
        public int UsageCount { get; set; } = 0;

        /// <summary>
        /// Thời gian trung bình để trả lời câu hỏi (giây)
        /// </summary>
        public double? AverageResponseTime { get; set; }

        /// <summary>
        /// Tỷ lệ thành công (phần trăm câu trả lời đúng)
        /// </summary>
        public double? SuccessRate { get; set; }

        /// <summary>
        /// SEO và tìm kiếm
        /// </summary>
        [StringLength(200)]
        public string Slug { get; set; }

        /// <summary>
        /// Chuỗi tìm kiếm đầy đủ (Full-Text Search Vector)
        /// </summary>
        public string SearchVector { get; set; } // For full-text search

        // Navigation properties

        /// <summary>
        /// Liên kết đến phần trong kỳ thi
        /// </summary>
        public virtual ICollection<SectionQuestion> SectionQuestions { get; set; }

        /// <summary>
        /// Các câu trả lời của người dùng
        /// </summary>
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        /// <summary>
        /// Các tệp phương tiện liên quan đến câu hỏi
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Các đáp án có thể tái sử dụng liên kết qua bảng trung gian QuestionOption
        /// </summary>
        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }

        // Business methods

        /// <summary>
        /// Kiểm tra loại câu hỏi
        /// Nếu là trắc nghiệm
        /// </summary>
        /// <returns></returns>
        public bool IsMultipleChoice() => QuestionType == QuestionTypeStatics.MULTIPLE_CHOICE;

        /// <summary>
        /// Kiểm tra loại câu hỏi
        /// Nếu là điền vào chỗ trống
        /// </summary>
        /// <returns></returns>
        public bool IsFillInBlank() => QuestionType == QuestionTypeStatics.FILL_IN_BLANK;

        /// <summary>
        /// Xét loại câu hỏi khác nếu cần
        /// Nếu là đúng/sai
        /// </summary>
        /// <returns></returns>
        public bool IsTrueFalse() => QuestionType == QuestionTypeStatics.TRUE_FALSE;

        /// <summary>
        /// Cập nhật thống kê sử dụng câu hỏi
        /// </summary>
        /// <param name="responseTime"></param>
        /// <param name="isCorrect"></param>
        public void UpdateUsageStats(double responseTime, bool isCorrect)
        {
            UsageCount++;

            // Update average response time
            if (AverageResponseTime.HasValue)
                AverageResponseTime = (AverageResponseTime.Value * (UsageCount - 1) + responseTime) / UsageCount;
            else
                AverageResponseTime = responseTime;

            // Update success rate (simplified calculation)
            if (SuccessRate.HasValue)
            {
                var totalCorrect = SuccessRate.Value * (UsageCount - 1) / 100;
                if (isCorrect) totalCorrect++;
                SuccessRate = (totalCorrect / UsageCount) * 100;
            }
            else
                SuccessRate = isCorrect ? 100 : 0;
        }
    }

    public static class QuestionTypeStatics
    {
        /// <summary>
        /// Loại câu hỏi trắc nghiệm
        /// </summary>
        public const string MULTIPLE_CHOICE = "MULTIPLE_CHOICE";

        /// <summary>
        /// Loại câu hỏi điền vào chỗ trống
        /// </summary>
        public const string FILL_IN_BLANK = "FILL_IN_BLANK";

        /// <summary>
        /// Loại câu hỏi đúng/sai
        /// </summary>
        public const string TRUE_FALSE = "TRUE_FALSE";
        // Add more types as needed
    }

}

