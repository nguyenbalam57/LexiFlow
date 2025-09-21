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

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Câu hỏi thi với các cải tiến về performance và audit
    /// </summary>
    [Index(nameof(QuestionType), Name = "IX_Question_Type")]
    [Index(nameof(SectionId), Name = "IX_Question_Section")]
    [Index(nameof(CreatedBy), Name = "IX_Question_CreatedBy")]
    [Index(nameof(IsActive), nameof(QuestionType), Name = "IX_Question_Active_Type")]
    [Index(nameof(IsDeleted), Name = "IX_Question_SoftDelete")]
    [Index(nameof(Difficulty), Name = "IX_Question_Difficulty")]
    [Index(nameof(Tags), Name = "IX_Question_Tags")] // For full-text search
    public class Question : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        public int? SectionId { get; set; }

        [Required]
        [StringLength(50)]
        public string QuestionType { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public string QuestionInstruction { get; set; }

        [StringLength(50)]
        public string Difficulty { get; set; } = "Medium";

        [Range(1, 100)]
        public int Points { get; set; } = 1;

        [Range(1, 3600)] // Max 1 hour per question
        public int TimeLimit { get; set; } = 60;

        [StringLength(1000)]
        public string CorrectAnswer { get; set; }

        public string Explanation { get; set; }

        [StringLength(500)]
        public string Tags { get; set; }

        // Performance tracking
        public int UsageCount { get; set; } = 0;
        public double? AverageResponseTime { get; set; }
        public double? SuccessRate { get; set; }

        // SEO và tìm kiếm
        [StringLength(200)]
        public string Slug { get; set; }
        public string SearchVector { get; set; } // For full-text search

        // Navigation properties
        [ForeignKey("SectionId")]
        public virtual JLPTSection Section { get; set; }

        public virtual ICollection<QuestionOption> Options { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }

        // IActivatable implementation
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Business methods
        public bool HasCorrectAnswer() => !string.IsNullOrWhiteSpace(CorrectAnswer);
        public bool IsMultipleChoice() => QuestionType?.ToUpper() == "MULTIPLE_CHOICE";
        public bool IsFillInBlank() => QuestionType?.ToUpper() == "FILL_IN_BLANK";

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
}

