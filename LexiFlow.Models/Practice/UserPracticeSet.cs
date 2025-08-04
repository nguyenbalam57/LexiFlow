using LexiFlow.Models.Analytics;
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

namespace LexiFlow.Models.Practice
{
    /// <summary>
    /// Kết quả luyện tập của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(PracticeSetId), Name = "IX_UserPractice_User_Set")]
    public class UserPracticeSet : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPracticeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PracticeSetId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? LastPracticed { get; set; }

        public int CompletionPercentage { get; set; } = 0;

        public int CorrectPercentage { get; set; } = 0;

        public int TotalAttempts { get; set; } = 0;

        // Cải tiến: Thống kê chi tiết
        public int TotalCorrect { get; set; } = 0;
        public int TotalIncorrect { get; set; } = 0;
        public int? TotalTime { get; set; }
        public int? AverageResponseTime { get; set; }

        // Cải tiến: Đánh giá của người dùng
        [Range(1, 5)]
        public int? UserRating { get; set; }

        public string UserFeedback { get; set; }

        public string UserNotes { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("PracticeSetId")]
        public virtual PracticeSet PracticeSet { get; set; }

        public virtual ICollection<UserPracticeAnswer> UserPracticeAnswers { get; set; }
        public virtual ICollection<PracticeAnalytic> PracticeAnalytics { get; set; }
    }
}
