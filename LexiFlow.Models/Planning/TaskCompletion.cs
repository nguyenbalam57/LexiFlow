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
    /// Thông tin hoàn thành nhiệm vụ học tập
    /// </summary>
    [Index(nameof(TaskId), nameof(CompletionDate), Name = "IX_TaskCompletion_Task_Date")]
    public class TaskCompletion : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompletionId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public DateTime CompletionDate { get; set; } = DateTime.UtcNow;

        public int? CompletionStatus { get; set; } // 0-100

        public int? ActualDuration { get; set; } // Thời gian thực tế

        [StringLength(20)]
        public string DurationUnit { get; set; } // Minutes, Hours

        // Cải tiến: Người hoàn thành
        public int? CompletedByUserId { get; set; } // Người hoàn thành

        // Cải tiến: Đánh giá nhiệm vụ
        [Range(1, 5)]
        public int? Difficulty { get; set; } // Mức độ khó (1-5)

        [Range(1, 5)]
        public int? Satisfaction { get; set; } // Mức độ hài lòng (1-5)

        [Range(1, 5)]
        public int? Effectiveness { get; set; } // Mức độ hiệu quả (1-5)

        // Cải tiến: Ghi chú và phân tích
        public string Notes { get; set; } // Ghi chú

        public string Feedback { get; set; } // Phản hồi

        // Cải tiến: Kết quả và thông tin học tập
        public string Outcome { get; set; } // Kết quả học tập

        public int? Score { get; set; } // Điểm số

        public int? CorrectCount { get; set; } // Số câu đúng

        public int? TotalCount { get; set; } // Tổng số câu

        // Cải tiến: Dữ liệu thực tế
        public string CompletionData { get; set; } // Dữ liệu hoàn thành (JSON)

        public string LearningMetrics { get; set; } // Số liệu học tập (JSON)

        // Cải tiến: Liên kết nội dung
        public int? RelatedEntityId { get; set; } // ID thực thể liên quan

        [StringLength(50)]
        public string RelatedEntityType { get; set; } // Loại thực thể liên quan

        // Navigation properties
        [ForeignKey("TaskId")]
        public virtual StudyTask Task { get; set; }

        [ForeignKey("CompletedByUserId")]
        public virtual User.User CompletedByUser { get; set; }
    }
}
