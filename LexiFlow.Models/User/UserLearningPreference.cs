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

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Tùy chọn học tập của người dùng
    /// </summary>
    [Index(nameof(UserId), IsUnique = true, Name = "IX_UserLearningPreference_User")]
    public class UserLearningPreference : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PreferenceId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Cài đặt học tập chung
        [Range(1, 100)]
        public int DailyNewWordsGoal { get; set; } = 10;

        [Range(1, 100)]
        public int DailyReviewsGoal { get; set; } = 30;

        [Range(5, 120)]
        public int StudySessionLengthMinutes { get; set; } = 20;

        // Phương pháp học
        [StringLength(50)]
        public string LearningStyle { get; set; } = "Balanced"; // Visual, Auditory, Reading, Balanced

        [StringLength(50)]
        public string StudyFocus { get; set; } = "Comprehensive"; // Vocabulary, Grammar, Reading, Listening, Comprehensive

        // Spaced repetition
        [StringLength(50)]
        public string SpacedRepetitionAlgorithm { get; set; } = "Default"; // Default, Simplified, Custom

        public double EaseFactorModifier { get; set; } = 1.0; // Điều chỉnh độ khó spaced repetition

        // Thông báo nhắc nhở
        public bool ShowHints { get; set; } = true;

        public bool ShowFurigana { get; set; } = true;

        // Cấp độ khó
        [StringLength(20)]
        public string PreferredDifficulty { get; set; } = "Adaptive"; // Easy, Normal, Hard, Adaptive

        // Cài đặt giao diện
        [StringLength(20)]
        public string ThemePreference { get; set; } = "System"; // Light, Dark, System

        [StringLength(20)]
        public string FontSize { get; set; } = "Medium"; // Small, Medium, Large

        // Quản lý nội dung
        public bool AutoDownloadAudio { get; set; } = true;

        public bool ShowExplicitContent { get; set; } = false;

        public bool PreferSimplifiedContent { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
