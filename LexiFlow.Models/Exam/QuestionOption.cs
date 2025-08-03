using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Lựa chọn cho câu hỏi
    /// </summary>
    public class QuestionOption : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionOptionId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public string OptionText { get; set; }

        [StringLength(255)]
        public string OptionImage { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? DisplayOrder { get; set; }

        // Cải tiến: Giải thích chi tiết
        public string Explanation { get; set; }

        // Cải tiến: Tỷ lệ chọn đáp án này
        public double? SelectionRate { get; set; }

        // Cải tiến: Mức độ nhiễu
        public int? DistractorLevel { get; set; }

        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        // Cải tiến: Navigation cho media
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
