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
    /// Tiến trình học tập Kanji của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(KanjiId), IsUnique = true, Name = "IX_UserKanjiProgress_User_Kanji")]
    public class UserKanjiProgress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int KanjiId { get; set; }

        public int RecognitionLevel { get; set; } = 0;  // Mức độ nhận biết (0-10)

        public int WritingLevel { get; set; } = 0;  // Mức độ viết (0-10)

        public DateTime? LastPracticed { get; set; }  // Thời điểm luyện tập gần nhất

        public int PracticeCount { get; set; } = 0;  // Số lần luyện tập

        public int CorrectCount { get; set; } = 0;  // Số lần trả lời đúng

        // Cải tiến: Spaced repetition
        public DateTime? NextReviewDate { get; set; }  // Thời điểm ôn tập tiếp theo

        public int MemoryStrength { get; set; } = 0; // Độ mạnh ghi nhớ

        public double EaseFactor { get; set; } = 2.5; // Hệ số dễ dàng

        public int IntervalDays { get; set; } = 1; // Khoảng thời gian ôn tập

        // Cải tiến: Thống kê chi tiết
        public int IncorrectCount { get; set; } = 0; // Số lần sai

        public int ConsecutiveCorrect { get; set; } = 0; // Số lần đúng liên tiếp

        public int? AverageResponseTimeMs { get; set; } // Thời gian phản hồi trung bình

        // Cải tiến: Tiến trình từng kỹ năng
        public int ReadingLevel { get; set; } = 0; // Mức độ đọc

        public int CompoundLevel { get; set; } = 0; // Mức độ kết hợp với Kanji khác

        public int MeaningLevel { get; set; } = 0; // Mức độ hiểu nghĩa

        // Cải tiến: Ghi chú và theo dõi
        public string Notes { get; set; }  // Ghi chú cá nhân

        public string LearningHistory { get; set; } // Lịch sử học tập dạng JSON

        public bool IsBookmarked { get; set; } = false; // Đánh dấu

        public int Priority { get; set; } = 3; // Mức độ ưu tiên học

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("KanjiId")]
        public virtual Learning.Kanji.Kanji Kanji { get; set; }
    }
}
