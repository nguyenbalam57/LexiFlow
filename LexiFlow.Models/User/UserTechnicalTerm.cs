using LexiFlow.Models.Core;
using LexiFlow.Models.Learning.TechnicalTerms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Liên kết giữa người dùng và thuật ngữ kỹ thuật
    /// </summary>
    [Index(nameof(UserId), nameof(TermId), IsUnique = true, Name = "IX_UserTechnicalTerm_User_Term")]
    public class UserTechnicalTerm : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTermId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TermId { get; set; }

        public int ProficiencyLevel { get; set; } = 0; // 0-10

        public DateTime? LastPracticed { get; set; }

        public int StudyCount { get; set; } = 0;

        public int CorrectCount { get; set; } = 0;

        // Cải tiến: Spaced repetition
        public DateTime? NextReviewDate { get; set; }

        public int MemoryStrength { get; set; } = 0;

        public double EaseFactor { get; set; } = 2.5;

        public int IntervalDays { get; set; } = 1;

        // Cải tiến: Thống kê chi tiết
        public int IncorrectCount { get; set; } = 0;

        public int ConsecutiveCorrect { get; set; } = 0;

        public int? AverageResponseTimeMs { get; set; }

        // Cải tiến: Mức độ ưu tiên và ghi chú
        public int Priority { get; set; } = 3; // 1-5

        public string UserNotes { get; set; }

        public bool IsBookmarked { get; set; } = false;

        // Cải tiến: Đánh giá chủ quan
        [Range(1, 5)]
        public int? UserDifficultyRating { get; set; }

        [Range(1, 5)]
        public int? UserImportanceRating { get; set; }

        // Cải tiến: Phân loại và gắn thẻ
        [StringLength(255)]
        public string UserTags { get; set; }

        [StringLength(50)]
        public string UserCategory { get; set; }

        // Cải tiến: Ví dụ cá nhân
        [StringLength(500)]
        public string PersonalExample { get; set; }

        [StringLength(255)]
        public string PersonalDefinition { get; set; }

        // Cải tiến: Lịch sử học tập
        public string LearningHistory { get; set; } // JSON format

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }
    }
}
