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
    /// Cấp độ JLPT
    /// </summary>
    [Index(nameof(LevelName), IsUnique = true, Name = "IX_JLPT_Level")]
    public class JLPTLevel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LevelId { get; set; }

        [Required]
        [StringLength(10)]
        public string LevelName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? VocabularyCount { get; set; }

        public int? KanjiCount { get; set; }

        public int? GrammarPoints { get; set; }

        public int? PassingScore { get; set; }

        // Cải tiến: Chi tiết kỹ năng
        public string RequiredSkills { get; set; }

        // Cải tiến: CEFR tương đương
        [StringLength(10)]
        public string CEFREquivalent { get; set; }

        // Cải tiến: Thời gian học tối thiểu
        public int? RecommendedStudyHours { get; set; }

        // Cải tiến: Yêu cầu trước khi học
        public int? PrerequisiteLevelId { get; set; }

        // Cải tiến: Thứ tự hiển thị
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        [ForeignKey("PrerequisiteLevelId")]
        public virtual JLPTLevel PrerequisiteLevel { get; set; }

        public virtual ICollection<Planning.StudyGoal> StudyGoals { get; set; }
        public virtual ICollection<JLPTExam> Exams { get; set; }
    }
}
