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
    /// Mục trong kế hoạch học tập
    /// </summary>
    [Index(nameof(PlanId), nameof(ItemType), Name = "IX_StudyPlanItem_Plan_Type")]
    public class StudyPlanItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; } // Vocabulary, Grammar, Kanji, Test, etc.

        public string Content { get; set; } // Nội dung chi tiết

        public DateTime? ScheduledDate { get; set; } // Ngày dự kiến

        // Cải tiến: Thông tin kế hoạch
        [Range(1, 5)]
        public int? Priority { get; set; } = 3; // Mức độ ưu tiên

        public int? EstimatedTime { get; set; } // Thời gian ước tính

        [StringLength(20)]
        public string TimeUnit { get; set; } = "Minutes"; // Đơn vị thời gian

        // Cải tiến: Liên kết với nội dung học tập
        public int? TopicId { get; set; } // Chủ đề

        public int? VocabularyGroupId { get; set; } // Nhóm từ vựng

        public int? GrammarId { get; set; } // Điểm ngữ pháp

        public int? ExamId { get; set; } // Bài kiểm tra

        // Cải tiến: Lịch trình và lặp lại
        public bool IsRecurring { get; set; } = false; // Lặp lại

        [StringLength(50)]
        public string RecurrencePattern { get; set; } // Mẫu lặp lại

        public int? RecurrenceCount { get; set; } // Số lần lặp lại

        // Cải tiến: Trạng thái và tiến trình
        public bool IsRequired { get; set; } = true; // Bắt buộc

        [StringLength(50)]
        public string Status { get; set; } = "Planned"; // Planned, InProgress, Completed, Skipped

        public float CompletionPercentage { get; set; } = 0; // Phần trăm hoàn thành

        // Cải tiến: Ghi chú và đánh dấu
        public string Notes { get; set; } // Ghi chú

        public bool IsHighlighted { get; set; } = false; // Đánh dấu nổi bật

        // Cải tiến: Điều kiện và phụ thuộc
        public string Dependencies { get; set; } // Phụ thuộc vào các mục khác

        public string CompletionRequirements { get; set; } // Yêu cầu để hoàn thành

        // Navigation properties
        [ForeignKey("PlanId")]
        public virtual StudyPlan Plan { get; set; }

        [ForeignKey("TopicId")]
        public virtual StudyTopic Topic { get; set; }

        [ForeignKey("VocabularyGroupId")]
        public virtual Learning.Vocabulary.VocabularyGroup VocabularyGroup { get; set; }

        [ForeignKey("GrammarId")]
        public virtual Learning.Grammar.Grammar Grammar { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam.JLPTExam Exam { get; set; }

        public virtual ICollection<StudyPlanProgress> StudyPlanProgresses { get; set; }
        public virtual ICollection<StudyTask> StudyTasks { get; set; }
    }
}
