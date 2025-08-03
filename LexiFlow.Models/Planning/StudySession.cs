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
    /// Phiên học tập của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(StartTime), Name = "IX_StudySession_User_StartTime")]
    public class StudySession : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public DateTime? EndTime { get; set; }

        public int Duration { get; set; } = 0; // Thời gian học tập tính bằng giây

        [StringLength(50)]
        public string SessionType { get; set; } // Study, Practice, Test, etc.

        [StringLength(50)]
        public string ContentType { get; set; } // Vocabulary, Kanji, Grammar, etc.

        public int? CategoryId { get; set; }

        public int? StudyPlanId { get; set; }

        public int ItemsStudied { get; set; } = 0;

        public int ItemsCompleted { get; set; } = 0;

        public int CorrectAnswers { get; set; } = 0;

        public int WrongAnswers { get; set; } = 0;

        public float Score { get; set; } = 0;

        [StringLength(255)]
        public string DeviceInfo { get; set; }

        [StringLength(50)]
        public string AppVersion { get; set; }

        [StringLength(50)]
        public string Platform { get; set; } // Web, Mobile, Desktop

        // Cải tiến: Thông tin mạng
        [StringLength(50)]
        public string NetworkType { get; set; } // WiFi, Cellular, Offline

        // Cải tiến: Thông tin chế độ học
        [StringLength(50)]
        public string StudyMode { get; set; } // Normal, Focused, Revision

        // Cải tiến: Khóa học liên quan
        public int? CourseId { get; set; }

        // Cải tiến: Thông tin vị trí
        [StringLength(255)]
        public string Location { get; set; }

        // Cải tiến: Bối cảnh học tập
        [StringLength(255)]
        public string StudyContext { get; set; }

        // Cải tiến: Trạng thái đồng bộ
        public bool IsSynced { get; set; } = false;

        public DateTime? SyncedAt { get; set; }

        // Cải tiến: Thời gian phản hồi trung bình
        public int? AverageResponseTimeMs { get; set; }

        // Cải tiến: Thông tin hiệu suất
        public string PerformanceMetrics { get; set; } // JSON data

        // Cải tiến: Phương thức đánh giá hiệu quả
        [StringLength(50)]
        public string EvaluationMethod { get; set; } // SRS, Quiz, Flashcard

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Learning.Vocabulary.Category Category { get; set; }

        [ForeignKey("StudyPlanId")]
        public virtual StudyPlan StudyPlan { get; set; }

        public virtual ICollection<Progress.LearningSessionDetail> SessionDetails { get; set; }
    }
}
