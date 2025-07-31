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
    /// Tiến trình học tập từ vựng
    /// </summary>
    [Index(nameof(UserId), nameof(VocabularyId), IsUnique = true, Name = "IX_User_Vocabulary")]
    [Index(nameof(UserId), nameof(NextReviewDate), Name = "IX_User_NextReview")]
    [Index(nameof(LastStudied), Name = "IX_LearningProgress_LastStudied")]
    [Index(nameof(NextReviewDate), Name = "IX_LearningProgress_NextReviewDate")]
    public class LearningProgress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        public int StudyCount { get; set; } = 0;
        public int CorrectCount { get; set; } = 0;
        public int IncorrectCount { get; set; } = 0;

        public DateTime? LastStudied { get; set; }

        // Cải tiến: Thuật toán spaced repetition
        public int MemoryStrength { get; set; } = 0;
        public double EaseFactor { get; set; } = 2.5;
        public int IntervalDays { get; set; } = 1;
        public int ConsecutiveCorrect { get; set; } = 0;

        public DateTime? NextReviewDate { get; set; }

        // Cải tiến: Theo dõi kỹ năng riêng biệt
        public int RecognitionLevel { get; set; } = 0; // Khả năng nhận biết (0-10)
        public int WritingLevel { get; set; } = 0; // Khả năng viết (0-10)
        public int ListeningLevel { get; set; } = 0; // Khả năng nghe (0-10)
        public int SpeakingLevel { get; set; } = 0; // Khả năng nói (0-10)

        // Cải tiến: Theo dõi thời gian phản hồi
        public int AverageResponseTimeMs { get; set; } = 0;
        public int LastResponseTimeMs { get; set; } = 0;

        public string LearningNotes { get; set; } // Ghi chú học tập

        // Cải tiến: Bookmark và ưu tiên
        public bool IsBookmarked { get; set; } = false;
        public int Priority { get; set; } = 3; // 1-5, 5 = cao nhất

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Learning.Vocabulary.Vocabulary Vocabulary { get; set; }
    }
}
