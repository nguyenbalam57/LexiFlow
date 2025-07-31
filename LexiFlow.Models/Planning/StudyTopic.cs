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
    /// Chủ đề học tập
    /// </summary>
    [Index(nameof(TopicName), nameof(Category), IsUnique = true, Name = "IX_StudyTopic_Name_Category")]
    public class StudyTopic : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }

        [Required]
        [StringLength(100)]
        public string TopicName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Category { get; set; } // Vocabulary, Grammar, General, etc.

        public int? ParentTopicId { get; set; }

        // Cải tiến: Phân loại và hiển thị
        [StringLength(255)]
        public string IconPath { get; set; } // Icon

        [StringLength(20)]
        public string ColorCode { get; set; } // Mã màu

        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        // Cải tiến: Thông tin học tập
        [StringLength(10)]
        public string JLPTLevel { get; set; } // N5, N4, etc.

        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3; // Mức độ khó

        public int? EstimatedStudyHours { get; set; } // Số giờ học ước tính

        // Cải tiến: Nội dung và liên kết
        public string KeyVocabulary { get; set; } // Từ vựng chính

        public string KeyGrammar { get; set; } // Ngữ pháp chính

        public string RelatedTopics { get; set; } // Chủ đề liên quan

        // Cải tiến: Tài nguyên và tài liệu
        public string LearningResources { get; set; } // Tài nguyên học tập

        public string RecommendedMaterials { get; set; } // Tài liệu đề xuất

        // Cải tiến: Thống kê và phân tích
        public int? UserCount { get; set; } = 0; // Số người dùng học chủ đề này

        public double? AverageCompletionDays { get; set; } // Số ngày hoàn thành trung bình

        public double? SuccessRate { get; set; } // Tỷ lệ thành công

        // Navigation properties
        [ForeignKey("ParentTopicId")]
        public virtual StudyTopic ParentTopic { get; set; }

        public virtual ICollection<StudyTopic> ChildTopics { get; set; }
        public virtual ICollection<StudyGoal> StudyGoals { get; set; }
        public virtual ICollection<StudyPlanItem> StudyPlanItems { get; set; }
    }
}
