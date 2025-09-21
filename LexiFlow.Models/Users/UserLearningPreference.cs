using LexiFlow.Models.Cores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Tùy chọn học tập của người dùng
    /// </summary>
    [Index(nameof(UserId), IsUnique = true, Name = "IX_UserLearningPreference_User")]
    public class UserLearningPreference : BaseEntity
    {
        /// <summary>
        /// Khóa chính của bảng tùy chọn học tập (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PreferenceId { get; set; }

        /// <summary>
        /// ID của người dùng mà tùy chọn này áp dụng.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        // =================== Cài đặt học tập chung ===================

        /// <summary>
        /// Số lượng từ mới mục tiêu mỗi ngày (1–100, mặc định 10).
        /// </summary>
        [Range(1, 100)]
        public int DailyNewWordsGoal { get; set; } = 10;

        /// <summary>
        /// Số lượng từ/cụm từ cần ôn tập mỗi ngày (1–100, mặc định 30).
        /// </summary>
        [Range(1, 100)]
        public int DailyReviewsGoal { get; set; } = 30;

        /// <summary>
        /// Thời lượng trung bình của một phiên học (5–120 phút, mặc định 20).
        /// </summary>
        [Range(5, 120)]
        public int StudySessionLengthMinutes { get; set; } = 20;

        // =================== Phương pháp học ===================

        /// <summary>
        /// Phong cách học tập ưa thích (Visual, Auditory, Reading, Balanced...).
        /// </summary>
        [StringLength(50)]
        public string LearningStyle { get; set; } = "Balanced";

        /// <summary>
        /// Trọng tâm học tập (Vocabulary, Grammar, Reading, Listening, Comprehensive).
        /// </summary>
        [StringLength(50)]
        public string StudyFocus { get; set; } = "Comprehensive";

        // =================== Spaced Repetition ===================

        /// <summary>
        /// Thuật toán spaced repetition sử dụng (Default, Simplified, Custom).
        /// </summary>
        [StringLength(50)]
        public string SpacedRepetitionAlgorithm { get; set; } = "Default";

        /// <summary>
        /// Hệ số điều chỉnh độ khó của spaced repetition (>= 0, mặc định 1.0).
        /// </summary>
        public double EaseFactorModifier { get; set; } = 1.0;

        // =================== Thông báo & hỗ trợ ===================

        /// <summary>
        /// Hiển thị gợi ý trong khi học.
        /// </summary>
        public bool ShowHints { get; set; } = true;

        /// <summary>
        /// Hiển thị Furigana (hỗ trợ ngôn ngữ có ký tự khó đọc).
        /// </summary>
        public bool ShowFurigana { get; set; } = true;

        // =================== Cấp độ & độ khó ===================

        /// <summary>
        /// Mức độ khó ưa thích (Easy, Normal, Hard, Adaptive).
        /// </summary>
        [StringLength(20)]
        public string PreferredDifficulty { get; set; } = "Adaptive";

        // =================== Cài đặt giao diện ===================

        /// <summary>
        /// Chủ đề hiển thị (Light, Dark, System).
        /// </summary>
        [StringLength(20)]
        public string ThemePreference { get; set; } = "System";

        /// <summary>
        /// Kích thước chữ trong ứng dụng (Small, Medium, Large).
        /// </summary>
        [StringLength(20)]
        public string FontSize { get; set; } = "Medium";

        // =================== Quản lý nội dung ===================

        /// <summary>
        /// Tự động tải xuống file âm thanh khi học.
        /// </summary>
        public bool AutoDownloadAudio { get; set; } = true;

        /// <summary>
        /// Cho phép hiển thị nội dung nhạy cảm/explicit.
        /// </summary>
        public bool ShowExplicitContent { get; set; } = false;

        /// <summary>
        /// Ưu tiên hiển thị nội dung đơn giản hóa.
        /// </summary>
        public bool PreferSimplifiedContent { get; set; } = false;

        // =================== Navigation properties ===================

        /// <summary>
        /// Người dùng mà tùy chọn học tập này thuộc về.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}

