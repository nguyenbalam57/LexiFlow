using LexiFlow.Models.Cores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Exams
{
    /// <summary>
    /// Cấp độ JLPT
    /// </summary>
    [Index(nameof(LevelName), IsUnique = true, Name = "IX_JLPT_Level")]
    public class JLPTLevel : AuditableEntity
    {
        /// <summary>
        /// Id cấp độ (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LevelId { get; set; }

        /// <summary>
        /// Tên cấp độ (N1, N2, N3, N4, N5)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LevelName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Số lượng từ vựng chuẩn
        /// </summary>
        public int? VocabularyCount { get; set; }

        /// <summary>
        /// Số lượng kanji chuẩn
        /// </summary>
        public int? KanjiCount { get; set; }

        /// <summary>
        /// Số lượng điểm ngữ pháp chuẩn
        /// </summary>
        public int? GrammarPoints { get; set; }

        /// <summary>
        /// Điểm đỗ
        /// </summary>
        public int? PassingScore { get; set; }

        /// <summary>
        /// Chi tiết kỹ năng
        /// </summary>
        public string RequiredSkills { get; set; }

        /// <summary>
        /// Thời gian học tối thiểu
        /// </summary>
        public int? RecommendedStudyHours { get; set; }

        /// <summary>
        /// Yêu cầu trước khi học
        /// </summary>
        public int? PrerequisiteLevelId { get; set; }

        // Navigation properties
        [ForeignKey("PrerequisiteLevelId")]
        public virtual JLPTLevel PrerequisiteLevel { get; set; }

        public virtual ICollection<JLPTExam> Exams { get; set; }
    }
}
