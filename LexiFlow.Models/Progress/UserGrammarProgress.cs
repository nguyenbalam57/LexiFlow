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

namespace LexiFlow.Models.Progress
{
    /// <summary>
    /// Tiến trình học tập ngữ pháp của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(GrammarId), IsUnique = true, Name = "IX_UserGrammarProgress_User_Grammar")]
    public class UserGrammarProgress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GrammarId { get; set; }

        public int UnderstandingLevel { get; set; } = 0; // Mức độ hiểu (0-10)

        public int UsageLevel { get; set; } = 0; // Mức độ sử dụng (0-10)

        public DateTime? LastStudied { get; set; } // Thời điểm học gần nhất

        public int StudyCount { get; set; } = 0; // Số lần học

        public float? TestScore { get; set; } // Điểm kiểm tra

        // Cải tiến: Spaced repetition
        public DateTime? NextReviewDate { get; set; } // Thời điểm ôn tập tiếp theo

        public int MemoryStrength { get; set; } = 0; // Độ mạnh ghi nhớ

        public double EaseFactor { get; set; } = 2.5; // Hệ số dễ dàng

        public int IntervalDays { get; set; } = 1; // Khoảng thời gian ôn tập

        // Cải tiến: Thống kê chi tiết
        public int CorrectCount { get; set; } = 0; // Số lần đúng

        public int IncorrectCount { get; set; } = 0; // Số lần sai

        public int ConsecutiveCorrect { get; set; } = 0; // Số lần đúng liên tiếp

        public int? AverageResponseTimeMs { get; set; } // Thời gian phản hồi trung bình

        // Cải tiến: Tiến trình theo kỹ năng
        public int RecognitionLevel { get; set; } = 0; // Mức độ nhận biết

        public int ProductionLevel { get; set; } = 0; // Mức độ tạo ra

        public int ContextualLevel { get; set; } = 0; // Mức độ hiểu ngữ cảnh

        // Cải tiến: Ghi chú và theo dõi
        public string PersonalNotes { get; set; } // Ghi chú cá nhân

        public string CommonMistakes { get; set; } // Lỗi thường gặp

        public string LearningHistory { get; set; } // Lịch sử học tập dạng JSON

        public bool IsBookmarked { get; set; } = false; // Đánh dấu

        public int Priority { get; set; } = 3; // Mức độ ưu tiên học

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("GrammarId")]
        public virtual Learning.Grammar.Grammar Grammar { get; set; }
    }
}
