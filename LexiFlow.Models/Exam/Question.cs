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

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Câu hỏi trong đề thi
    /// </summary>
    [Index(nameof(SectionId), nameof(QuestionType), Name = "IX_Question_Section_Type")]
    [Index(nameof(CreatedByUserId), Name = "IX_Question_CreatedBy")]
    public class Question : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        public int? SectionId { get; set; }

        [StringLength(50)]
        public string QuestionType { get; set; }

        public string QuestionText { get; set; }

        [StringLength(255)]
        public string QuestionImage { get; set; }

        [StringLength(255)]
        public string QuestionAudio { get; set; }

        [StringLength(255)]
        public string CorrectAnswer { get; set; }

        public string Explanation { get; set; }

        public int? Difficulty { get; set; }

        [StringLength(10)]
        public string JLPT_Level { get; set; }

        // Cải tiến: Điểm cho câu hỏi
        public int? Points { get; set; } = 1;

        // Cải tiến: Mức độ phân biệt
        public double? DiscriminationIndex { get; set; }

        // Cải tiến: Thống kê về câu hỏi
        public int? AttemptCount { get; set; } = 0;
        public int? CorrectCount { get; set; } = 0;

        // Cải tiến: Thời gian trung bình trả lời
        public int? AverageTimeSeconds { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        [StringLength(255)]
        public string Skills { get; set; }

        public bool IsVerified { get; set; } = false;

        public int? CreatedByUserId { get; set; }

        // Navigation properties
        [ForeignKey("SectionId")]
        public virtual JLPTSection Section { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User.User CreatedByUser { get; set; }

        public virtual ICollection<QuestionOption> Options { get; set; }
        public virtual ICollection<Practice.CustomExamQuestion> CustomExamQuestions { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<Practice.PracticeSetItem> PracticeSetItems { get; set; }
        public virtual ICollection<Practice.UserPracticeAnswer> UserPracticeAnswers { get; set; }

        // Cải tiến: Navigation cho media
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
