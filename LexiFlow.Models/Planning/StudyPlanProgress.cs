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
    /// Tiến trình thực hiện kế hoạch học tập
    /// </summary>
    [Index(nameof(ItemId), Name = "IX_StudyPlanProgress_Item")]
    public class StudyPlanProgress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        [Required]
        public int ItemId { get; set; }

        // Cải tiến: Thông tin trạng thái
        public int? CompletionStatus { get; set; } // 0-100

        public DateTime? CompletedDate { get; set; } // Ngày hoàn thành

        [StringLength(50)]
        public string Status { get; set; } = "NotStarted"; // NotStarted, InProgress, Completed, Skipped

        // Cải tiến: Thời gian thực tế
        public int? ActualTime { get; set; } // Thời gian thực tế

        [StringLength(20)]
        public string TimeUnit { get; set; } = "Minutes"; // Đơn vị thời gian

        public DateTime? StartedAt { get; set; } // Thời điểm bắt đầu

        public DateTime? FinishedAt { get; set; } // Thời điểm kết thúc

        // Cải tiến: Thông tin chi tiết
        [Range(1, 5)]
        public int? Difficulty { get; set; } // Mức độ khó (nhận xét của người dùng)

        [Range(1, 5)]
        public int? Satisfaction { get; set; } // Mức độ hài lòng

        [Range(1, 5)]
        public int? Effectiveness { get; set; } // Mức độ hiệu quả

        // Cải tiến: Ghi chú và phân tích
        public string UserNotes { get; set; } // Ghi chú của người dùng

        public string Challenges { get; set; } // Thách thức gặp phải

        public string Improvements { get; set; } // Đề xuất cải tiến

        // Cải tiến: Thống kê học tập
        public int? ItemsCompleted { get; set; } // Số mục đã hoàn thành

        public int? CorrectAnswers { get; set; } // Số câu trả lời đúng

        public int? TotalQuestions { get; set; } // Tổng số câu hỏi

        public float? Score { get; set; } // Điểm số

        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual StudyPlanItem Item { get; set; }
    }
}
