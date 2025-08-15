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
    /// Câu hỏi thi
    /// </summary>
    [Index(nameof(QuestionType), Name = "IX_Question_Type")]
    [Index(nameof(SectionId), Name = "IX_Question_Section")]
    [Index(nameof(CreatedByUserId), Name = "IX_Question_CreatedBy")]
    public class Question : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        public int? SectionId { get; set; }

        [Required]
        [StringLength(50)]
        public string QuestionType { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public string QuestionInstruction { get; set; }

        [StringLength(50)]
        public string Difficulty { get; set; } = "Medium";

        public int Points { get; set; } = 1;

        public int TimeLimit { get; set; } = 60;

        [StringLength(255)]
        public string CorrectAnswer { get; set; }

        public string Explanation { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        public bool IsActive { get; set; } = true;

        public int? CreatedByUserId { get; set; }

        // Navigation properties
        [ForeignKey("SectionId")]
        public virtual JLPTSection Section { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User.User CreatedByUser { get; set; }

        public virtual ICollection<QuestionOption> Options { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
