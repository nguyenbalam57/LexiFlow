using LexiFlow.Models.Core;
using LexiFlow.Models.Learning.Vocabulary;
using LexiFlow.Models.Planning;
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
    /// Phiên học tập
    /// </summary>
    [Index(nameof(UserId), nameof(StartTime), Name = "IX_User_Session")]
    public class LearningSession : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int DurationMinutes { get; set; }

        [StringLength(50)]
        public string SessionType { get; set; } // Vocabulary, Grammar, Kanji, etc.

        public int ItemsStudied { get; set; }
        public int CorrectAnswers { get; set; }
        public int MistakesMade { get; set; }

        public double EfficiencyScore { get; set; } // Điểm hiệu suất học tập

        [StringLength(50)]
        public string Platform { get; set; } // Web, Mobile, Desktop

        [StringLength(100)]
        public string DeviceInfo { get; set; }

        // Cải tiến: Phiên học theo chủ đề
        public int? CategoryId { get; set; }

        // Cải tiến: Phiên học với mục tiêu
        public int? GoalId { get; set; }

        public int? GroupId { get; set; }

        // Cải tiến: Theo dõi cảm xúc
        [StringLength(50)]
        public string MoodBefore { get; set; }

        [StringLength(50)]
        public string MoodAfter { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Learning.Vocabulary.Category Category { get; set; }

        [ForeignKey("GoalId")]
        public virtual StudyGoal Goal { get; set; }

        [ForeignKey("GroupId")]
        public virtual VocabularyGroup VocabularyGroup { get; set; }

        public virtual ICollection<LearningSessionDetail> SessionDetails { get; set; }
    }
}
