using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Progress
{
    /// <summary>
    /// Chi tiết phiên học tập
    /// </summary>
    public class LearningSessionDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; } // Vocabulary, Kanji, Grammar

        public int ItemId { get; set; }

        public bool WasCorrect { get; set; }

        public int ResponseTimeMs { get; set; } // Thời gian phản hồi

        public int AttemptCount { get; set; } = 1;

        [StringLength(255)]
        public string UserAnswer { get; set; }

        // Cải tiến: Độ khó mỗi lần ôn tập
        [StringLength(20)]
        public string Difficulty { get; set; } // Easy, Good, Hard

        // Cải tiến: Chi tiết trạng thái học tập
        public int MemoryStrengthBefore { get; set; }
        public int MemoryStrengthAfter { get; set; }
        public double EaseFactorBefore { get; set; }
        public double EaseFactorAfter { get; set; }

        // Navigation property
        [ForeignKey("SessionId")]
        public virtual LearningSession Session { get; set; }
    }
}
